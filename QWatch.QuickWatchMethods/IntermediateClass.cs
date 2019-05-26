using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Configuration;
using ObjToXml;
using System.IO;
using DataContext;
using Entity;

namespace QuickWatchMethods
{
    public class IntermediateClass
    {
        public string RefreshData(string _defaultDate)
        {
            string _masterDatapathOI = ConfigurationManager.AppSettings["MasterDatapathOI"];
            string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
            string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
            string _masterDMAExcelSource = ConfigurationManager.AppSettings["MasterDMAExcelSource"];

            BasicDataLogic objList = new BasicDataLogic();
            string outputMessageName = AddMasterData(_masterDMAExcelSource, objList.UpdateDMAMasterSource(_masterDatapathBasic, _defaultDate));
            return outputMessageName;
        }
        private string AddMasterData(string mastersource, List<CompanyDetails> companyDetails)
        {
            string message = "";
            DateTime toDayDate = DateTime.Now;
            string dayname = toDayDate.DayOfWeek.ToString();
            if (dayname == "Sunday" || dayname == "Saturday")
            {
                return message = "Sunday & Saturday you can't update data.";
            }
            string today = toDayDate.ToString("MM/dd/yyyy").Trim().ToLower();


            var companyDetailsList = companyDetails.Where(x => x.Code != null).ToList();
            DataLoader objData = new DataLoader();
            List<string> files = objData.GetAllFile(mastersource, "");
            List<string> completedFiles = new List<string>();
            List<string> FailedFiles = new List<string>();

            try
            {
                foreach (var item in companyDetailsList)
                {
                    string name = item.Code;
                    string ltpPrice = item.lastPrice;
                    string fileName = "\\" + name + ".csv";
                    var filePath = files.Where(x => x.Contains(fileName)).ToList();
                    if (filePath.Count > 0)
                    {
                        string text = System.IO.File.ReadAllText(filePath.First().ToString());
                        if (!(text.Trim().ToLower().Contains(today)))
                        {
                            using (StreamWriter sw = new StreamWriter(filePath.First().ToString(), true))
                            {
                                DateTime oDate = DateTime.Parse(item.Date);
                                sw.WriteLine(string.Concat(item.Code + ",", "EQ" + ",", oDate.ToString("MM/dd/yyyy") + ",", item.PreClose.Replace(",", "") + ",", item.Open.Replace(",", "") + ",", item.High.Replace(",", "") + ",", item.Low.Replace(",", "") + ",", item.lastPrice.Replace(",", "") + ",", item.Closed.Replace(",", "") + ",", item.Closed.Replace(",", "") + ",", item.Closed.Replace(",", "") + ",", item.Closed.Replace(",", "") + ",", item.Closed.Replace(",", "") + ",", item.Closed.Replace(",", "") + ",", item.Closed.Replace(",", "") + ","));
                                sw.Close();
                                sw.Dispose();
                            }
                            completedFiles.Add(filePath.First().ToString());
                        }
                        else
                        {
                            FailedFiles.Add(fileName);
                        }
                    }
                }
                if (completedFiles.Count == 167 && FailedFiles.Count == 0)
                {
                    message = "Updated sucussfully.";
                }
                else if (completedFiles.Count == 0 && FailedFiles.Count == 167)
                {
                    message = "Not updated as data existed in files for today";
                }
                else if (completedFiles.Count > 0 && FailedFiles.Count > 0)
                {
                    message = "Something went wrong, kinldy check the system status.";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }
        public string ProcessRequest(string id)
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
                        xmltoObj.CreateXML(objData.BasicData, _masterDatapathBasic, "BasicData",false);


                        List<KeyValue> objLtpPrices = GenerateLastPrices(objData.BasicData);
                        objData.DMAData = objDma.GetDmaData(_masterDMAExcelSource, objLtpPrices, "");
                        message = message + " DMA,";
                        xmltoObj.CreateXML(objData.DMAData, _masterDatapathDMA, "DMAData", false);
                    }
                    if (item == "DMA")
                    {
                        //DMA moved to Basic data refresh, when you click referesh request for the 
                        //basic data then system refresh DMA data also
                        //So, no need to saparate request for the DMA.
                        //Presently signing off the DMA refresh option based on the data.csv file

                        objData.DMAData = objDma.GetDmaData(_masterDMAExcelSource, _TodayCSVFile, "");
                        message = message + " DMA,";
                        xmltoObj.CreateXML(objData.DMAData, _masterDatapathDMA, "DMAData", false);
                    }
                    if (item == "OI")
                    {
                        IOData objOI = new IOData();
                        objData.OIData = objOI.LoadOIData(_nseUrlIo, _expiryDate, _list);
                        message = message + " OI,";
                        xmltoObj.CreateXML(objData.OIData, _masterDatapathOI, "OIData", false);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }
        private List<KeyValue> GenerateLastPrices(List<CompanyDetails> data)
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
        public string LoadMCData()
        {
            string message = string.Empty;
            try
            {
                string masterMCpath = ConfigurationManager.AppSettings["MasterMCpath"];
                MC objData = new MC();
                OBJtoXML xmltoObj = new OBJtoXML();
                List<MCData> data = objData.LoadData();
                xmltoObj.CreateXML(data, masterMCpath, "MCData", false);
                message = "Loaded MC Data";
            }

            catch(Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }
    }
}
