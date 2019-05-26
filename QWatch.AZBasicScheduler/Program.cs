using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Configuration;
using ObjToXml;
using QuickWatchMethods;
using Entity;
using DataContext;


namespace AZBasicScheduler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Console.WriteLine("Process Started AZBasicScheduler.");
            var config = new JobHostConfiguration();
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            LoadBasicData("BasicData,OI");
            Console.WriteLine("Loaded BasicData,OI data sucussfully.");
            Console.WriteLine("Thread locked.");
            System.Threading.Thread.Sleep(180000);
            Console.WriteLine("Thread lock released.");
            host.RunAndBlock();
        }
        private static List<KeyValue> GenerateLastPrices(List<CompanyDetails> data)
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
        public static string LoadBasicData(string id)
        {
            string message = "Sucussfully loaded data for ";
            string _nseUrlIo = ConfigurationManager.AppSettings["NseUrlIO"].Replace("&amp;", "&");
            string _nseUrlBasic = ConfigurationManager.AppSettings["NseUrlBasic"].Replace("&amp;", "&");
            string _nifty50 = ConfigurationManager.AppSettings["nifty50"].Replace("&amp;", "&");
            string _allFO = ConfigurationManager.AppSettings["AllFO"].Replace("&amp;", "&");
            string _indexOnly = ConfigurationManager.AppSettings["IndexOnly"].Replace("&amp;", "&");
            string _banknifty = ConfigurationManager.AppSettings["Banknifty"].Replace("&amp;", "&");
            string _expiryDate = ConfigurationManager.AppSettings["ExpiryDate"];
            string _masterDMAExcelSource = ConfigurationManager.AppSettings["MasterDMAExcelSource"];
            string _TodayCSVFile = ConfigurationManager.AppSettings["TodayCSVFile"];
            string _masterDatapathOI = ConfigurationManager.AppSettings["MasterDatapathOI"];
            string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
            string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];



            BulkEntity objData = new BulkEntity();
            Dma objDma = new Dma();
            OBJtoXML xmltoObj = new OBJtoXML();

            try
            {
                string _list = "";

                if (id.Contains("IndexOnly"))
                {
                    _list = _indexOnly;
                    message = message + " IndexOnly,";
                }
                else if (id.Contains("Banknifty"))
                {
                    _list = _banknifty;
                    message = message + " Banknifty,";
                }
                else if (id.Contains("nifty50"))
                {
                    _list = _nifty50;
                    message = message + " nifty50,";
                }
                else
                {
                    _list = _allFO;
                    message = message + " All listed items.,";
                }

                foreach (string item in id.Split(','))
                {
                    if (item == "BasicData")
                    {
                        objData.BasicData = BasicData.ColletData(_nseUrlBasic, _list);
                        message = message + " BasicData,";
                        xmltoObj.CreateXML(objData.BasicData, _masterDatapathBasic, "basicdata", true);

                        //Need to implement this one when master data been ready
                        //List<KeyValue> objLtpPrices = GenerateLastPrices(objData.BasicData);
                        //objData.DMAData = objDma.GetDmaData(_masterDMAExcelSource, objLtpPrices, "");
                        //message = message + " DMA,";
                        //xmltoObj.CreateXML(objData.DMAData, _masterDatapathDMA, "dmadata", true);
                    }
                    if (item == "OI")
                    {
                        IOData objOI = new IOData();
                        objData.OIData = objOI.LoadOIData(_nseUrlIo, _expiryDate, _list);
                        message = message + " OI,";
                        xmltoObj.CreateXML(objData.OIData, _masterDatapathOI, "openinterest", true);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }
    }
}