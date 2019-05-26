using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Log;
using System.IO;
using System.Configuration;
using ObjToXml;
using QuickWatchMethods;
using Entity;
using DataContext;

namespace QWService
{
    public partial class QWService : ServiceBase
    {
        public QWService()
        {
            InitializeComponent();
        }

        System.Timers.Timer _timer = new System.Timers.Timer();
        protected override void OnStart(string[] args)
        {
            MessageContext.MessageLog("QW Service Started");
            _timer.Interval = Convert.ToDouble(120000);
            _timer.Elapsed += mainTimer_Elapsed;
            _timer.Start();
        }
        void mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            ProcessData();
            _timer.Start();
        }
       
        public void ProcessData()
        {
            try
            {
                string[] _logfilePath = ConfigurationManager.AppSettings["timeToLoadData"].Split(',');
                string[] _updateDMADataNow = ConfigurationManager.AppSettings["UpdateDMADataNow"].Split(',');
                string[] _loadDataNow = ConfigurationManager.AppSettings["LoadDataNow"].Split(',');
                string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
                string _masterDMAExcelSource = ConfigurationManager.AppSettings["MasterDMAExcelSource"];
                string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];

                string _defaultDate = "0";


                TimeSpan now = DateTime.Now.TimeOfDay;
                foreach (string item in _logfilePath)
                {
                    int h = Convert.ToInt32(item.Split('.')[0]);
                    int s = Convert.ToInt32(item.Split('.')[1]);

                    TimeSpan start = new TimeSpan(h, s, 0);
                    TimeSpan end = new TimeSpan(h, s + 3, 0);

                    if ((now > start) && (now < end))
                    {
                        //Service processing the request in Market time
                        IntermediateClass obj = new IntermediateClass();
                        MessageContext.MessageLog(obj.ProcessRequest("BasicData,OI"));
                        MessageContext.MessageLog(obj.LoadMCData());
                    }



                    if (_loadDataNow.ToString().ToUpper() == "TRUE")
                    {
                        //Service processing the request in Market time
                        IntermediateClass obj = new IntermediateClass();
                        MessageContext.MessageLog(obj.ProcessRequest("BasicData,OI-Using Loaded now option."));
                        MessageContext.MessageLog(obj.LoadMCData());
                    }
                }

                //post market time service updating the Master data excel files
                TimeSpan startPost = new TimeSpan(17, 0, 0);
                TimeSpan endPost = new TimeSpan(20, 0, 0);
                if ((now > startPost) && (now < endPost))
                {
                    IntermediateClass obj = new IntermediateClass();
                    string message = obj.RefreshData(_defaultDate);
                    if (message.Contains("Updated sucussfully"))
                    {
                        try
                        {
                            BasicDataLogic objList = new BasicDataLogic();
                            Dma objDma = new Dma();
                            OBJtoXML xmltoObj = new OBJtoXML();

                            List<CompanyDetails> basicData = objList.UpdateDMAMasterSource(_masterDatapathBasic, _defaultDate);
                            List<KeyValue> objLtpPrices = GeneratePrices(basicData);
                            List<Rank> dMADataRank = objDma.GetDmaData(_masterDMAExcelSource, objLtpPrices, "");
                            message = message + " DMA,";
                            xmltoObj.CreateXML(dMADataRank, _masterDatapathDMA, "DMAData", false);
                        }
                        catch (Exception ex)
                        {
                            ErrorContext.ErrorLog("Error in PostRefresh DMA Data" + ex.Message);
                        }

                    }
                    MessageContext.MessageLog(message);
                }
                if (_updateDMADataNow.ToString().ToUpper() == "TRUE")
                {
                    IntermediateClass obj = new IntermediateClass();
                    MessageContext.MessageLog(obj.RefreshData(_defaultDate));
                    MessageContext.MessageLog("DMA data updated using updateDMADataNow option.");
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                ErrorContext.ErrorLog("Problem when Gettign the files   " + ex.Message.ToString());
            }
        }
        private List<KeyValue> GeneratePrices(List<CompanyDetails> data)
        {
            List<KeyValue> objList = new List<KeyValue>();
            KeyValue obj = null;
            foreach (var item in data.Where(x => x.Code != null && x.lastPrice != null))
            {
                obj = new KeyValue();
                obj.KeyName = item.Code;
                obj.Value = item.lastPrice;
                obj.ClosedPrice = item.Closed;
                objList.Add(obj);
            }
            return objList;
        }
        protected override void OnStop()
        {
            MessageContext.MessageLog("QW Service service stopped");
        }
    }
}
