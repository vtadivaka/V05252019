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
using OIOData;
using Entity;

namespace AnalyzerService
{
    public partial class AnalyzerService : ServiceBase
    {
        public AnalyzerService()
        {
            InitializeComponent();
        }

        System.Timers.Timer _timer = new System.Timers.Timer();
        protected override void OnStart(string[] args)
        {
            MessageContext.MessageLog("Options Service Started");
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
                string _masterDMAExcelSource = ConfigurationManager.AppSettings["MasterDMAExcelSource"];
                string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
                string _masterDatapathBasic = ConfigurationManager.AppSettings["bnUrl"];
                string _bnCurrentExpiryDate = ConfigurationManager.AppSettings["BnNextExpiryDate"];
                string _bnBNSeriousValue = ConfigurationManager.AppSettings["BNSeriousValue"];
                string _bnCurrentUrlValue = ConfigurationManager.AppSettings["CurrentUrlValue"];
                int _count = Convert.ToInt32(ConfigurationManager.AppSettings["IndexNumbers"]);


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

                        try
                        {
                            MessageContext.MessageLog("Started Loading for options");
                            NIndex();
                            BNCurrentIndex();
                            BNNextIndex();
                            MessageContext.MessageLog("Completed Loading for options");
                            try
                            {
                                MessageContext.MessageLog("Started Loading for Stk options");
                                Stk();
                                MessageContext.MessageLog("Completed Loading for Stk options ");
                            }
                            catch (Exception ex)
                            {
                                ErrorContext.ErrorLog("Error in Stk()" + ex.Message.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorContext.ErrorLog("Error Loading for options   " + ex.Message.ToString());
                        }
                    }
                }

                //post market time service updating the Master data excel files
                TimeSpan startPost = new TimeSpan(17, 0, 0);
                TimeSpan endPost = new TimeSpan(20, 0, 0);
                if ((now > startPost) && (now < endPost))
                {
                  
                }
               
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                ErrorContext.ErrorLog("Problem when Gettign the files   " + ex.Message.ToString());
            }
        }

        public void Stk()
        {
            string _masterDatapathBasic = ConfigurationManager.AppSettings["bnUrl"];
            string _pathOptionsData = ConfigurationManager.AppSettings["StkOptions"];
            string _expiryDate = ConfigurationManager.AppSettings["ExpiryDate"];
            string _nSeriousValue = ConfigurationManager.AppSettings["NSeriousValue"];
            string _nCurrentUrlValue = ConfigurationManager.AppSettings["StkUrlBasic"];
            int _count = Convert.ToInt32(ConfigurationManager.AppSettings["StkNumbers"]);
            List<string> objStkList = IOData.StPrice.StkPrices();
            List<OIOData.OIDetails> optionsData = new List<OIDetails>();
            foreach (var item in objStkList)
            {
                string _url = _nCurrentUrlValue.Replace("EXPIRYCODE", _expiryDate).Replace("COMPANYNAME", item.Split('#')[0].Trim());
                QueryBinder dataList = new QueryBinder();
                OptionsIO obj = new OptionsIO();
                optionsData.AddRange(obj.LoadOptionsData(_masterDatapathBasic.Replace("BANKNIFTY", item.Split('#')[0].Trim()).Replace("OPTIDX", "OPTSTK"), _expiryDate, _url,Convert.ToSingle(item.Split('#')[1].Trim()), _count));
            }
           

            OBJtoXML xmltoObj = new OBJtoXML();
            xmltoObj.CreateOptionsXML(optionsData, _pathOptionsData, "StkOptions");
        }

        private void NIndex()
        {
            string _masterDatapathBasic = ConfigurationManager.AppSettings["bnUrl"];
            string _pathOptionsData = ConfigurationManager.AppSettings["NOptions"];
            string _expiryDate = ConfigurationManager.AppSettings["ExpiryDate"];
            string _nSeriousValue = ConfigurationManager.AppSettings["NSeriousValue"];
            string _nCurrentUrlValue = ConfigurationManager.AppSettings["bnUrl"];
            int _count = Convert.ToInt32(ConfigurationManager.AppSettings["IndexNumbers"]);
            

            string _url = _nCurrentUrlValue.Replace("EXPIRYCODE", _expiryDate).Replace("TYPECEPE","CE").Replace("INDEXVALUE", _nSeriousValue).Replace("BANKNIFTY", "NIFTY");
            QueryBinder dataList = new QueryBinder();
            OptionsIO obj = new OptionsIO();
            List<OIOData.OIDetails> optionsData = obj.LoadOptionsData(_masterDatapathBasic, _expiryDate, _url, 50F, _count);

            OBJtoXML xmltoObj = new OBJtoXML();
            xmltoObj.CreateOptionsXML(optionsData, _pathOptionsData, "NIndexOptions");
        }
        private void BNCurrentIndex()
        {
            string _pathOptionsData = ConfigurationManager.AppSettings["BNCurrentOptions"];
            string _masterDatapathBasic = ConfigurationManager.AppSettings["bnUrl"];
            string _bnCurrentExpiryDate = ConfigurationManager.AppSettings["BnCurrentExpiryDate"];
            string _bnBNSeriousValue = ConfigurationManager.AppSettings["BNSeriousValue"];
            string _bnCurrentUrlValue = ConfigurationManager.AppSettings["bnUrl"];
            int _count = Convert.ToInt32(ConfigurationManager.AppSettings["IndexNumbers"]);

            string _url = _bnCurrentUrlValue.Replace("EXPIRYCODE", _bnCurrentExpiryDate).Replace("TYPECEPE", "CE").Replace("INDEXVALUE", _bnBNSeriousValue);
            QueryBinder dataList = new QueryBinder();
            OptionsIO obj = new OptionsIO();
            List<OIOData.OIDetails> optionsData = obj.LoadOptionsData(_masterDatapathBasic, _bnCurrentExpiryDate, _url, 100F, _count);

            OBJtoXML xmltoObj = new OBJtoXML();
            xmltoObj.CreateOptionsXML(optionsData, _pathOptionsData, "BNCurrentIndex");
        }

        public void BNNextIndex()
        {
            string _pathOptionsData = ConfigurationManager.AppSettings["BNNextOptions"];
            string _masterDatapathBasic = ConfigurationManager.AppSettings["bnUrl"];
            string _bnCurrentExpiryDate = ConfigurationManager.AppSettings["BnNextExpiryDate"];
            string _bnBNSeriousValue = ConfigurationManager.AppSettings["BNSeriousValue"];
            string _bnCurrentUrlValue = ConfigurationManager.AppSettings["bnUrl"];
            int _count = Convert.ToInt32(ConfigurationManager.AppSettings["IndexNumbers"]);

            string _url = _bnCurrentUrlValue.Replace("EXPIRYCODE", _bnCurrentExpiryDate).Replace("TYPECEPE", "CE").Replace("INDEXVALUE", _bnBNSeriousValue);
            QueryBinder dataList = new QueryBinder();
            OptionsIO obj = new OptionsIO();
            List<OIOData.OIDetails> optionsData = obj.LoadOptionsData(_masterDatapathBasic, _bnCurrentExpiryDate, _url, 100F, _count);

            OBJtoXML xmltoObj = new OBJtoXML();
            xmltoObj.CreateOptionsXML(optionsData, _pathOptionsData, "BNNextIndex");
        }

        protected override void OnStop()
        {
            MessageContext.MessageLog("Options service stopped");
        }
    }
}
