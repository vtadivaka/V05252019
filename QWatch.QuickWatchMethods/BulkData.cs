using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjToXml;
using System.IO;
using System.Collections;
using System.Globalization;
using DataContext;
using Entity;
using DataContext;

namespace QuickWatchMethods
{
  public class BulkData
    {
        private List<string> FilterDates(List<string> files, string _defaultDate)
        {
            List<string> list = new List<string>();
            bool value = false;
            foreach (var item in files)
            {
                if (value == true)
                {
                    list.Add(item);
                }
                else
                {
                    if (item.Trim().ToLower().Contains(_defaultDate.Trim().ToLower()))
                    {
                        value = true;
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        public List<string> GetCodes(string basicfilePath, string ioFilePath, string _defaultDate)
        {
            List<string> objList = new List<string>();
            BulkData obj = new BulkData();
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.BasicData = new List<CompanyDetails>();
            string[] txtFiles = Directory.GetFiles(basicfilePath, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false));break;
            }

            foreach (var item in datafromXML.BasicData)
            {
                if(!(objList.Contains(item.Code)))
                {
                    objList.Add(item.Code);
                }
            }


         
            datafromXML.OIData = new List<OIDetails>();
            var txtFilesOIDetails = Directory.EnumerateFiles(ioFilePath, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            foreach (string currentFile in txtFilesOIDetails)
            {
                datafromXML.OIData.AddRange((List<OIDetails>)xmltoObj.CreateObject(datafromXML.OIData, currentFile, false)); break;
            }

            foreach (var item in datafromXML.OIData)
            {
                if (!(objList.Contains(item.CompanyCode)))
                {
                    objList.Add(item.CompanyCode);
                }
            }
            return objList;
        }

        public List<CompanyDetails> GetBasicData(List<Weightage> weightageData,string companyCode,string path, string _defaultDate)
        {
            List<CompanyDetails> returnData = new List<CompanyDetails>();
            if (companyCode == "Discounting")
            {
                BulkData obj = new BulkData();
                // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
                ////test to read data
                OBJtoXML xmltoObj = new OBJtoXML();
                BulkEntity datafromXML = new BulkEntity();
                datafromXML.BasicData = new List<CompanyDetails>();
                string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

                //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
                List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
                foreach (string currentFile in filesList)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false));break;
                }
                List<CompanyDetails> queryData = new List<CompanyDetails>();
                returnData = datafromXML.BasicData.OrderBy(x => x.BasicSavedTimeStamp).ToList();
            }
            else if (companyCode == "All")
            {
                BulkData obj = new BulkData();
                // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
                ////test to read data
                OBJtoXML xmltoObj = new OBJtoXML();
                BulkEntity datafromXML = new BulkEntity();
                datafromXML.BasicData = new List<CompanyDetails>();
                string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

                //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
                List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
                foreach (string currentFile in filesList)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false));
                }
                List<CompanyDetails> queryData = new List<CompanyDetails>();
                queryData = datafromXML.BasicData.OrderBy(x => x.BasicSavedTimeStamp).ToList();
                returnData = CalcFastMoversForAll(companyCode, queryData.Where(x => x.TotalVolume != null && x.totalBuyQuantity != "1" && x.totalSellQuantity != "1" ).ToList());
             }
            else if(companyCode.Split(',')[0].ToUpper() == "BANKNIFTY")
            {
                BulkData obj = new BulkData();
                OBJtoXML xmltoObj = new OBJtoXML();
                BulkEntity datafromXML = new BulkEntity();
                datafromXML.BasicData = new List<CompanyDetails>();
                string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

                List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
                foreach (string currentFile in filesList)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false));
                }
                List<CompanyDetails> queryData = new List<CompanyDetails>();
                queryData = datafromXML.BasicData.OrderBy(x => x.BasicSavedTimeStamp).ToList();
                returnData = CalcFastMoversForBankNifty(weightageData, companyCode, queryData.Where(x => x.TotalVolume != null && x.totalBuyQuantity != "1" && x.totalSellQuantity != "1").ToList());
            }
            else
            {
                BulkData obj = new BulkData();
                // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
                ////test to read data
                OBJtoXML xmltoObj = new OBJtoXML();
                BulkEntity datafromXML = new BulkEntity();
                datafromXML.BasicData = new List<CompanyDetails>();
                string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
                List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
                foreach (string currentFile in filesList)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false));
                }
                List<CompanyDetails> queryData = new List<CompanyDetails>();
                queryData = datafromXML.BasicData.OrderBy(x => x.BasicSavedTimeStamp).ToList();
                returnData=CalcFastMoversBasedOnCode(queryData.Where(x => x.TotalVolume != null && x.totalBuyQuantity != "1" && x.totalSellQuantity != "1" && x.Code == companyCode).ToList());
            }

            return returnData;
        }


        public List<CompanyDetails> GetBasicAvgAll(List<Weightage> weightageData,string path, string _defaultDate)
        {
            List<CompanyDetails> queryData = new List<CompanyDetails>();
            BulkData obj = new BulkData();
                // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
                ////test to read data
                OBJtoXML xmltoObj = new OBJtoXML();
                BulkEntity datafromXML = new BulkEntity();
                datafromXML.BasicData = new List<CompanyDetails>();
                string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
                List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
                foreach (string currentFile in filesList)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false));
                }
                
            queryData = datafromXML.BasicData.OrderByDescending(x => x.BasicSavedTimeStamp).ToList();
            return queryData;
        }

        private List<CompanyDetails> CalcFastMoversForBankNifty(List<Weightage> weightageData, string code, List<CompanyDetails> queryData)
        {
            List<CompanyDetails> MasterData = new List<CompanyDetails>();
            List<CompanyDetails> newqueryData = new List<CompanyDetails>();
            var companysList = code.Split(',').ToList();
            foreach (var cmpCode in companysList)
            {
                newqueryData = queryData.Where(x => x.Code == cmpCode).ToList();
                List<CompanyDetails> objData = new List<CompanyDetails>();
                CompanyDetails cmpDetails = null;
                string stringToReplace = "\"}],\"";
               var data = newqueryData.GroupBy(x => x.BasicSavedTimeStamp.Date).ToList();
                decimal previousDayMaxValue = 0.0M;
                decimal maxValue = 0.0M;
                decimal fastPercentage = 0.0M;
                int fastId = 0;
                int Id = 0;
                foreach (var item in data)
                {
                    //temp fix issue that removing the special chars from the total volume and this can remove
                    //after Nov15th
                    // var filteredList = item.Where(x => x.TotalVolume != null && x.totalBuyQuantity != "1" && x.totalSellQuantity != "1").ToList();
                    // if (item.Count > 0)
                    //  { 
                    previousDayMaxValue = maxValue;
                    maxValue = item.Max(x => Convert.ToDecimal(x.TotalVolume.Replace(stringToReplace, "")));
                    Id = 0;
                    fastId = 0;
                    foreach (var collection in item)
                    {
                        cmpDetails = new CompanyDetails();
                        cmpDetails.BasicSavedTimeStamp = collection.BasicSavedTimeStamp;
                        cmpDetails.change = collection.change;
                        cmpDetails.Closed = collection.Closed;
                        cmpDetails.Code = collection.Code;
                        cmpDetails.CompanyName = collection.CompanyName;
                        cmpDetails.Date = collection.Date;
                        cmpDetails.Delivery = collection.Delivery;
                        cmpDetails.DeliveryVolume = collection.DeliveryVolume;
                        cmpDetails.High = collection.High;
                        cmpDetails.high52 = collection.high52;
                        cmpDetails.lastPrice = collection.lastPrice;
                        cmpDetails.Low = collection.Low;
                        cmpDetails.low52 = collection.low52;
                        cmpDetails.Open = collection.Open;
                        cmpDetails.pChange = collection.pChange;
                        cmpDetails.Percentage = collection.Percentage;
                        cmpDetails.PreClose = collection.PreClose;
                        cmpDetails.totalBuyQuantity = collection.totalBuyQuantity;
                        cmpDetails.totalSellQuantity = collection.totalSellQuantity;
                        cmpDetails.TotalVolume = collection.TotalVolume.Replace(stringToReplace, "");
                        cmpDetails.UpdateToday = collection.UpdateToday;
                        cmpDetails.BNWeigtage = weightageData.Where(x=>x.Name.ToUpper() == collection.Code.ToUpper()).ToList()[0].Value;
                        cmpDetails.Id = Id; Id++;
                        if (previousDayMaxValue != 0.0M)
                        {
                            cmpDetails.CurrentPrevdayVolumePercentage = Math.Round(((Convert.ToDecimal(cmpDetails.TotalVolume) * 100) / previousDayMaxValue), 2).ToString();
                            if (fastId != 0)
                            {
                                cmpDetails.fastPercentage = Math.Round(((Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage) * 100) / fastPercentage - 100), 2).ToString();
                            }
                            fastPercentage = Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage);
                        }
                        fastId++;
                        cmpDetails.CalcDayVolume = String.Format(new CultureInfo("en-IN", false), "{0:n}", maxValue); //{0,n} used for inject the comma's in number
                        objData.Add(cmpDetails);
                    }

                    // }
                }
                ;
                MasterData.AddRange(objData.OrderByDescending(x => x.BasicSavedTimeStamp).ToList());
            }
            return MasterData.Where(x=>x.BasicSavedTimeStamp.Date == DateTime.Now.Date) .ToList();
           
        }
        private List<CompanyDetails> CalcFastMoversForAll(string code,List<CompanyDetails> queryData)
        {
            List<CompanyDetails> MasterData = new List<CompanyDetails>();
            List<CompanyDetails> newqueryData = new List<CompanyDetails>();
            var companysList = queryData.Select(x => new { x.Code }).Distinct().ToList(); ;
            foreach (var cmpCode in companysList)
            {
                newqueryData=queryData.Where(x => x.Code== cmpCode.Code).ToList();
                List<CompanyDetails> objData = new List<CompanyDetails>();
                CompanyDetails cmpDetails = null;
                string stringToReplace = "\"}],\"";
                var data = newqueryData.GroupBy(x => x.BasicSavedTimeStamp.Date).ToList();
                decimal previousDayMaxValue = 0.0M;
                decimal maxValue = 0.0M;
                decimal fastPercentage = 0.0M;
                int fastId = 0;
                int Id = 0;
                foreach (var item in data)
                {
                    previousDayMaxValue = maxValue;
                    maxValue = item.Max(x => Convert.ToDecimal(x.TotalVolume.Replace(stringToReplace, "")));
                    Id = 0;
                    fastId = 0;
                    foreach (var collection in item)
                    {
                        cmpDetails = new CompanyDetails();
                        cmpDetails.BasicSavedTimeStamp = collection.BasicSavedTimeStamp;
                        cmpDetails.change = collection.change;
                        cmpDetails.Closed = collection.Closed;
                        cmpDetails.Code = collection.Code;
                        cmpDetails.CompanyName = collection.CompanyName;
                        cmpDetails.Date = collection.Date;
                        cmpDetails.Delivery = collection.Delivery;
                        cmpDetails.DeliveryVolume = collection.DeliveryVolume;
                        cmpDetails.High = collection.High;
                        cmpDetails.high52 = collection.high52;
                        cmpDetails.lastPrice = collection.lastPrice;
                        cmpDetails.Low = collection.Low;
                        cmpDetails.low52 = collection.low52;
                        cmpDetails.Open = collection.Open;
                        cmpDetails.pChange = collection.pChange;
                        cmpDetails.Percentage = collection.Percentage;
                        cmpDetails.PreClose = collection.PreClose;
                        cmpDetails.totalBuyQuantity = collection.totalBuyQuantity;
                        cmpDetails.totalSellQuantity = collection.totalSellQuantity;
                        cmpDetails.TotalVolume = collection.TotalVolume.Replace(stringToReplace, "");
                        cmpDetails.UpdateToday = collection.UpdateToday;
                        cmpDetails.Id = Id; Id++;
                        if (previousDayMaxValue != 0.0M)
                        {
                            cmpDetails.CurrentPrevdayVolumePercentage = Math.Round(((Convert.ToDecimal(cmpDetails.TotalVolume) * 100) / previousDayMaxValue), 2).ToString();
                            if (fastId != 0 && fastPercentage != 0)
                            {
                                cmpDetails.fastPercentage = Math.Round(((Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage) * 100) / fastPercentage - 100), 2).ToString();
                            }
                            fastPercentage = Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage);
                        }
                        fastId++;
                        cmpDetails.CalcDayVolume = String.Format(new CultureInfo("en-IN", false), "{0:n}", maxValue); //{0,n} used for inject the comma's in number
                        objData.Add(cmpDetails);
                    }

                    // }
                }
                ;
                MasterData.AddRange(objData.OrderByDescending(x => x.BasicSavedTimeStamp).ToList());
            }
            return MasterData.ToList();
        }
        private List<CompanyDetails> CalcFastMoversBasedOnCode(List<CompanyDetails> queryData)
        {
            List<CompanyDetails> objData = new List<CompanyDetails>();
            CompanyDetails cmpDetails = null;
            string stringToReplace = "\"}],\"";
            var data = queryData.GroupBy(x=>x.BasicSavedTimeStamp.Date).ToList();
            decimal previousDayMaxValue = 0.0M;
            decimal maxValue = 0.0M;
            decimal fastPercentage = 0.0M;
            int fastId = 0;
            int Id = 0;
            foreach (var item in data)
            {
                       previousDayMaxValue = maxValue;
                       maxValue = item.Max(x => Convert.ToDecimal(x.TotalVolume.Replace(stringToReplace, "")));
                       Id = 0;
                       fastId = 0;
                foreach (var collection in item)
                    {   
                        cmpDetails = new CompanyDetails();
                        cmpDetails.BasicSavedTimeStamp = collection.BasicSavedTimeStamp;
                        cmpDetails.change = collection.change;
                        cmpDetails.Closed = collection.Closed;
                        cmpDetails.Code = collection.Code;
                        cmpDetails.CompanyName = collection.CompanyName;
                        cmpDetails.Date = collection.Date;
                        cmpDetails.Delivery = collection.Delivery;
                        cmpDetails.DeliveryVolume = collection.DeliveryVolume;
                        cmpDetails.High = collection.High;
                        cmpDetails.high52 = collection.high52;
                        cmpDetails.lastPrice = collection.lastPrice;
                        cmpDetails.Low = collection.Low;
                        cmpDetails.low52 = collection.low52;
                        cmpDetails.Open = collection.Open;
                        cmpDetails.pChange = collection.pChange;
                        cmpDetails.Percentage = collection.Percentage;
                        cmpDetails.PreClose = collection.PreClose;
                        cmpDetails.totalBuyQuantity = collection.totalBuyQuantity;
                        cmpDetails.totalSellQuantity = collection.totalSellQuantity;
                        cmpDetails.TotalVolume = collection.TotalVolume.Replace(stringToReplace, "");
                        cmpDetails.UpdateToday = collection.UpdateToday;
                        cmpDetails.Id = Id; Id++;
                    if (previousDayMaxValue != 0.0M)
                        {
                           cmpDetails.CurrentPrevdayVolumePercentage = Math.Round(((Convert.ToDecimal(cmpDetails.TotalVolume) * 100) / previousDayMaxValue),2).ToString();
                           if (fastId != 0 && fastPercentage > 0)
                            {
                           cmpDetails.fastPercentage = Math.Round(((Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage) * 100) / fastPercentage-100), 2).ToString();
                        }
                        fastPercentage = Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage);
                    }
                    fastId++;
                        cmpDetails.CalcDayVolume = String.Format(new CultureInfo("en-IN", false), "{0:n}", maxValue); //{0,n} used for inject the comma's in number
                        objData.Add(cmpDetails);
                }
            }

            if(queryData.Count != objData.Count)
            {
               throw new Exception("Error in counting the CalcDayVolume value");
            }
            return objData.OrderByDescending(x => x.BasicSavedTimeStamp).ToList();
        }


        public List<OIDetails> GetOpenInterestData(string code, string path, string _defaultDate)
        {
            BulkData obj = new BulkData();
            // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.OIData = new List<OIDetails>();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.OIData.AddRange((List<OIDetails>)xmltoObj.CreateObject(datafromXML.OIData, currentFile, false));
                if (code == "Discounting")
                {
                    break;
                }
            }
            List<OIDetails> queryData = new List<OIDetails>();
            if (code == "Discounting")
            {
                queryData = datafromXML.OIData.OrderByDescending(x => x.OISavedTimeStamp).ToList();
            }
            else
            {
                queryData = datafromXML.OIData.Where(x => x.CompanyCode == code).OrderByDescending(x => x.OISavedTimeStamp).ToList();
            }
             
            //return BankNiftyMoving(queryData);
            return queryData;
        }
        public List<OIDetails> GetOIAvgAll(string path, string _defaultDate)
        {
            List<OIDetails> queryData = new List<OIDetails>();
               BulkData obj = new BulkData();
            // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.OIData = new List<OIDetails>();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.OIData.AddRange((List<OIDetails>)xmltoObj.CreateObject(datafromXML.OIData, currentFile, false));
            }
           
                queryData = datafromXML.OIData.OrderByDescending(x => x.OISavedTimeStamp).ToList();
            //return BankNiftyMoving(queryData);
            return queryData;
        }
        
        private List<OIDetails> BankNiftyMoving(List<OIDetails> queryData)
        {
            List<OIDetails> objData = new List<OIDetails>();
            OIDetails cmpDetails = null;
            string stringToReplace = "\"}],\"";
            var data = queryData.GroupBy(x => x.OISavedTimeStamp.Date).ToList();
            decimal previousDayMaxValue = 0.0M;
            decimal maxValue = 0.0M;
            decimal fastPercentage = 0.0M;
            int fastId = 0;
            int Id = 0;
            foreach (var item in data)
            {
                previousDayMaxValue = maxValue;
                maxValue = item.Max(x => Convert.ToDecimal(x.NumberOfContractsTraded.Replace(stringToReplace, "")));
                Id = 0;
                fastId = 0;
                foreach (var collection in item)
                {
                    cmpDetails = new OIDetails();
                    cmpDetails.OISavedTimeStamp = collection.OISavedTimeStamp;
                    cmpDetails.PchangeinOpenInterest = collection.PchangeinOpenInterest;
                    cmpDetails.PrevClose = collection.PrevClose;
                    cmpDetails.CompanyCode = collection.CompanyCode;
                    cmpDetails.ExpiryDate = collection.ExpiryDate;
                    cmpDetails.LowPrice = collection.LowPrice;
                    cmpDetails.LastPrice = collection.LastPrice;
                    cmpDetails.totalBuyQuantity = collection.totalBuyQuantity;
                    cmpDetails.totalSellQuantity = collection.totalSellQuantity;
                    cmpDetails.HighPrice = collection.HighPrice;
                    cmpDetails.UnderlyingValue = collection.UnderlyingValue;
                    cmpDetails.PChange = collection.PChange;
                    cmpDetails.NumberOfContractsTraded = collection.NumberOfContractsTraded;
                   
                    cmpDetails.Id = Id; Id++;

                    if (previousDayMaxValue != 0.0M)
                    {
                        cmpDetails.CurrentPrevdayVolumePercentage = Math.Round(((Convert.ToDecimal(cmpDetails.NumberOfContractsTraded) * 100) / previousDayMaxValue), 2).ToString();
                        if (fastId != 0 && fastPercentage > 0)
                        {
                            cmpDetails.fastPercentage = Math.Round(((Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage) * 100) / fastPercentage - 100), 2).ToString();
                        }
                        fastPercentage = Convert.ToDecimal(cmpDetails.CurrentPrevdayVolumePercentage);
                    }
                    fastId++;
                    cmpDetails.CalcDayVolume = String.Format(new CultureInfo("en-IN", false), "{0:n}", maxValue); //{0,n} used for inject the comma's in number
                    objData.Add(cmpDetails);
                }
            }

            if (queryData.Count != objData.Count)
            {
                throw new Exception("Error in counting the BN Moving");
            }
            return objData.OrderByDescending(x => x.OISavedTimeStamp).ToList();
        }

        public List<Rank> GetDMAData(string code, string path, string _defaultDate)
        {
            BulkData obj = new BulkData();
            // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.DMAData = new List<Rank>();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false));
            }
            List<Rank> queryData = datafromXML.DMAData.Where(x => x.SourceName == code).OrderByDescending(x => x.RankSavedTimeStamp).ToList();
            return queryData;
        }

        public List<UIDetailedDMA> GetConvertObjectToDMAData(string code, string path, string _defaultDate)
        {
            BulkData obj = new BulkData();
            // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            ConvertiontoDMADayData objNewListObj = new ConvertiontoDMADayData();
            datafromXML.DMAData = new List<Rank>();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false));
            }
            List<Rank> queryData = datafromXML.DMAData.Where(x => x.SourceName == code).OrderByDescending(x => x.RankSavedTimeStamp).ToList();
            return objNewListObj.ConvertOnjectToDMAData(queryData);
        }


        public List<MCData> MCData(string path, string _defaultDate)
        {
            BulkData obj = new BulkData();
            // BulkEntity data = obj.AnalyzerData("BHARTIARTL");
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            List<MCData> data = new List<MCData>();

            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                data=(List<MCData>)xmltoObj.CreateObject(data, currentFile, false);break;
            }
            return data;
        }
    }
}
