using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjToXml;
using System.IO;
using System.Collections;
using Entity;
using DataContext;

namespace QuickWatchMethods
{
    public class BasicDataLogic
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
        public List<CompanyDetails> CheckBuySignals(string _masterDatapathBasic, string _defaultDate)
        {
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.BasicData = new List<CompanyDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false)); break;
            }

            List<CompanyDetails> queryData = datafromXML.BasicData.Where(x => Convert.ToDecimal(x.Low) == Convert.ToDecimal(x.Open)).ToList();
            return queryData;
        }
        public List<CompanyDetails> CheckSellSignals(string _masterDatapathBasic,string _defaultDate)
        {
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.BasicData = new List<CompanyDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime);
            foreach (string currentFile in txtFiles)
            {
                datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false)); break;
            }

            List<CompanyDetails> queryData = datafromXML.BasicData.Where(x => Convert.ToDecimal(x.High) == Convert.ToDecimal(x.Open)).ToList();
            return queryData;
        }

        private bool checkIsPostMarketData(string currentFile)
        {
            bool value = false;
            FileInfo fi = new FileInfo(currentFile);
            // var created = fi.CreationTime;
            DateTime lastmodified = fi.LastWriteTime;
            string day = lastmodified.DayOfWeek.ToString();

            TimeSpan start = new TimeSpan(9, 15, 0); //10 o'clock
            TimeSpan end = new TimeSpan(15, 30, 0); //12 o'clock
            TimeSpan now = lastmodified.TimeOfDay;

            if ((now > start) && (now < end))
            {
                value = true;
            }
            return value;
        }

        public List<CompanyDetails> CheckHighBuying(string _masterDatapathBasic, string _defaultDate)
        {
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.BasicData = new List<CompanyDetails>();

          var txtFiles = Directory.GetFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime);

            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                if (checkIsPostMarketData(currentFile) == true)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false)); break;
                }
            }

            var query1 = datafromXML.BasicData.Where(x => Convert.ToDecimal(x.totalBuyQuantity) > Convert.ToDecimal(x.totalSellQuantity))
                                              .Select(x =>
                                              new
                                              {
                                                  x.lastPrice,
                                                  x.CompanyName,
                                                  x.High,
                                                  x.Low,
                                                  x.Open,
                                                  Code = x.Code,
                                                  x.totalBuyQuantity,
                                                  x.totalSellQuantity,
                                                  Pchange = (((Convert.ToDecimal(x.lastPrice) / (Convert.ToDecimal(x.PreClose))) * 100) - 100),  //High must change with current price
                                                  Percentage = (((Convert.ToDecimal(x.totalBuyQuantity) / (Convert.ToDecimal(x.totalSellQuantity))) * 100) - 100).ToString()
                                              }).ToList()
                                              .OrderByDescending(x => Convert.ToDecimal(x.Percentage))
                                              .ToList();

            List<CompanyDetails> newlist = new List<CompanyDetails>();
            CompanyDetails cmp = null;
            foreach (var item in query1)
            {
                cmp = new CompanyDetails();
                cmp.Code = item.Code;
                cmp.totalBuyQuantity = item.totalBuyQuantity;
                cmp.totalSellQuantity = item.totalSellQuantity;
                cmp.pChange = item.Pchange.ToString();
                cmp.Percentage = item.Percentage;
                cmp.CompanyName = item.CompanyName;
                cmp.High = item.High;
                cmp.Low = item.Low;
                cmp.Open = item.Open;
                cmp.lastPrice = item.lastPrice;
                newlist.Add(cmp);
            }

            return newlist;
        }

        public List<CompanyDetails> CheckHighSelling(string _masterDatapathBasic, string _defaultDate)
        {

            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.BasicData = new List<CompanyDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                if (checkIsPostMarketData(currentFile) == true)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false)); break;
                }
            }

            var query2 = datafromXML.BasicData.Where(x => Convert.ToDecimal(x.totalBuyQuantity) < Convert.ToDecimal(x.totalSellQuantity))
                                              .Select(x =>
                                              new
                                              {
                                                  x.TotalVolume,
                                                  x.lastPrice,
                                                  x.CompanyName,
                                                  x.High,
                                                  x.Low,
                                                  x.Open,
                                                  Code = x.Code,
                                                  x.totalBuyQuantity,
                                                  x.totalSellQuantity,
                                                  Pchange = (((Convert.ToDecimal(x.lastPrice) / (Convert.ToDecimal(x.PreClose))) * 100) - 100), //Low must change with current price
                                                  Percentage = (((Convert.ToDecimal(x.totalBuyQuantity) / (Convert.ToDecimal(x.totalSellQuantity))) * 100) - 100).ToString()
                                              }).ToList()
                                              .OrderBy(x => Convert.ToDecimal(x.Percentage))
                                              .ToList();
            List<CompanyDetails> newlist = new List<CompanyDetails>();
            CompanyDetails cmp = null;
            foreach (var item in query2)
            {
                cmp = new CompanyDetails();
                cmp.Code = item.Code;
                cmp.totalBuyQuantity = item.totalBuyQuantity;
                cmp.totalSellQuantity = item.totalSellQuantity;
                cmp.pChange = item.Pchange.ToString();
                cmp.Percentage = item.Percentage;
                cmp.CompanyName = item.CompanyName;
                cmp.High = item.High;
                cmp.Low = item.Low;
                cmp.Open = item.Open;
                cmp.lastPrice = item.lastPrice;
                cmp.TotalVolume = item.TotalVolume;
                newlist.Add(cmp);
            }
            return newlist;
        }

        public List<CompanyDetails> UpdateDMAMasterSource(string _masterDatapathBasic, string _defaultDate)
        {
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.BasicData = new List<CompanyDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                if (checkIsPostMarketData(currentFile) == false)
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false)); break;
                }
            }
            return datafromXML.BasicData;
        }
    }
}
