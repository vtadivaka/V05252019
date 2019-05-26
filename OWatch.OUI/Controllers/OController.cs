using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using OIOData;
using OUI.Models;
using Entity;
using System.IO;
using ObjToXml;


namespace OUI.Controllers
{
    public class OController : Controller
    {

        public ActionResult StkIndex(string id)
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);


            string path = ConfigurationManager.AppSettings["StkOptions"];

            List<OIOData.OIDetails> objforXml = new List<OIDetails>();
            OBJtoXML xmltoObj = new OBJtoXML();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            List<OIOData.OIDetails> optionsData = new List<OIDetails>();
            foreach (string currentFile in filesList)
            {
                optionsData = (List<OIOData.OIDetails>)xmltoObj.CreateOptionsObject(objforXml, currentFile); 
            }
            QueryBinder dataList = new QueryBinder();
            optionsData = optionsData.Where(x=>x.underlying!=null).ToList();
            optionsData = optionsData.Where(x => x.underlying.ToUpper() == id.ToUpper()).ToList();
            dataList.StkCallData = GenerateCallData(optionsData).CallOptionsNData;
            dataList.StkPutData = GeneratePutData(optionsData).PutOptionsNData;

            return View(dataList);
        }
        public ActionResult Index()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);


            List<string> strSources = new List<string>();
            List<string> masterSources = IOData.StPrice.StkPrices();
            foreach (var item in masterSources)
            {
                strSources.Add(item.Split('#')[0].Trim().ToUpper());
            }
            TempData["StkList"] = strSources;

            return View();
        }
        // GET: O
        public ActionResult BNNextIndex()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);


            string path = ConfigurationManager.AppSettings["BNNextOptions"];

            List<OIOData.OIDetails> objforXml = new List<OIDetails>();
            OBJtoXML xmltoObj = new OBJtoXML();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList =FilterDates(txtFiles.ToList(), _defaultDate);
            List<OIOData.OIDetails> optionsData = new List<OIDetails>();
            foreach (string currentFile in filesList)
            {
                optionsData = (List<OIOData.OIDetails>)xmltoObj.CreateOptionsObject(objforXml, currentFile); 
            }
            QueryBinder dataList = new QueryBinder();

            dataList.CallOptionsNextBNData = GenerateCallData(optionsData).CallOptionsNData;
            dataList.PutOptionsNextBNData = GeneratePutData(optionsData).PutOptionsNData;

            return View(dataList);
        }

        public ActionResult BNCurrentIndex()
        {

            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);


            string path = ConfigurationManager.AppSettings["BNCurrentOptions"];

            List<OIOData.OIDetails> objforXml = new List<OIDetails>();
            OBJtoXML xmltoObj = new OBJtoXML();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            List<OIOData.OIDetails> optionsData = new List<OIDetails>();
            foreach (string currentFile in filesList)
            {
                optionsData = (List<OIOData.OIDetails>)xmltoObj.CreateOptionsObject(objforXml, currentFile); 
            }
            QueryBinder dataList = new QueryBinder();


            dataList.CallOptionsCurrentBNData = GenerateCallData(optionsData).CallOptionsNData;
            dataList.PutOptionsCurrentBNData = GeneratePutData(optionsData).PutOptionsNData;

            return View(dataList);
        }
       
        public ActionResult NIndex()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            string path= ConfigurationManager.AppSettings["NOptions"];

            List<OIOData.OIDetails> objforXml = new List<OIDetails>();
            OBJtoXML xmltoObj = new OBJtoXML();
            string[] txtFiles = Directory.GetFiles(path, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            int count = 0;

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            List<OIOData.OIDetails> optionsData = new List<OIDetails>();
            foreach (string currentFile in filesList)
            {
                optionsData.AddRange((List<OIOData.OIDetails>)xmltoObj.CreateOptionsObject(objforXml, currentFile)); 
            }
            QueryBinder dataList = new QueryBinder();
            dataList.CallOptionsNData = GenerateCallData(optionsData).CallOptionsNData;
            dataList.PutOptionsNData = GeneratePutData(optionsData).PutOptionsNData;

            return View(dataList);
        }
        private List<string> FilterDates(List<string> files, string _defaultDate)
        {
            List<string> list = new List<string>();
            foreach (var item in files)
            {
               if (item.Trim().ToLower().Contains(_defaultDate.Trim().ToLower()))
                    {
                        list.Add(item);
                    }
            }
            return list;
        }
        
        private void GenerateBackDateURLs(List<string> dates)
        {
            string url = HttpContext.Request.Url.AbsoluteUri;
            List<string> _backDateUrl = new List<string>();
            for (int i = 0; i < dates.Count; i++)
            {
                if (Request.QueryString["Date"] == null || Request.QueryString["Date"] == "0")
                {
                    if (i == 0)
                    {
                        _backDateUrl.Add(url + "?Date=-" + i + "#" + i + "Day (" + dates[i] + ")");
                    }
                    else
                    {
                        _backDateUrl.Add(url + "?Date=-" + i + "#" + i + "Day");
                    }
                }
                else
                {
                    if (Math.Abs(Convert.ToInt32(Request.QueryString["Date"])) == i)
                    {
                        _backDateUrl.Add(url + "?Date=-" + i + "#" + i + "Day (" + dates[i] + ")");
                    }
                    else
                    {
                        _backDateUrl.Add(url + "?Date=-" + i + "#" + i + "Day");
                    }
                }

            }
            TempData["Urls"] = _backDateUrl;
        }
        public List<OIDetails> GenerateToolTip(List<OIDetails> optionsData)
        {
            List<OIDetails> objList = new List<OIDetails>();
            OIDetails objItem = null;
            var newData = optionsData.GroupBy(x => x.strikePrice + x.optionType);
            int count = newData.Count()/2;
            int CurrentSeries = 0;

            foreach (var item in newData)
            {
                CurrentSeries++;
                objItem = new OIDetails();
                objItem = item.ToList()[0];
                if(CurrentSeries > count && CurrentSeries< count+4)
                {
                    objItem.BackColor = "darkgray"; 
                }else
                {
                    objItem.BackColor = "none";
                }

                var data = item.Select(x => x.changeinOpenInterest).ToList();
                objItem.changeinOpenInterest = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());
                
               // data = item.Select(x => x.strikePrice).ToList();
                //objItem.strikePrice = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.pchangeinOpenInterest).ToList();
                objItem.pchangeinOpenInterest = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.openInterest).ToList();
                objItem.openInterest = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.vwap).ToList();
                objItem.vwap = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.lastPrice).ToList();
                objItem.lastPrice = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.prevClose).ToList();
                objItem.prevClose = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.openPrice).ToList();
                objItem.openPrice = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.highPrice).ToList();
                objItem.highPrice = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());

                data = item.Select(x => x.lowPrice).ToList();
                objItem.lowPrice = data[0] + "#" + string.Join(Environment.NewLine, data.ToArray());
                objList.Add(objItem);
            }

            var countData = objList.Sum(x =>Convert.ToInt64(x.openInterest.Split('#')[0].Replace(",","")));
            objList[0].TotalCount = countData.ToString();
            return objList;
        }

        private List<string> DateCollection()
        {
            string _masterDatapathBasic = ConfigurationManager.AppSettings["NOptions"];
            string[] _basic = Directory.GetFiles(_masterDatapathBasic, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
         
            List<string> datesbasic = new List<string>();
            List<string> datesDma = new List<string>();

            foreach (var item in _basic)
            {
                string[] values = item.Split('-');
                string date = values[1] + "-" + values[2] + "-" + values[3];
                if (!datesbasic.Contains(date))
                {
                    datesbasic.Add(date);
                }
            }
          
            return datesbasic;
        }
        private List<string> GetAvaliableDates(out string displayDate)
        {
            List<string> dates = DateCollection();
            if (Request.QueryString["Date"] == null || Request.QueryString["Date"] == "0")
            {
                displayDate = dates[0].ToString();
            }
            else
            {
                displayDate = dates[Math.Abs(Convert.ToInt32(Request.QueryString["Date"]))].ToString();
            }
            return dates;
        }
        public QueryBinder GeneratePutData(List<OIDetails> optionsData)
        {
            QueryBinder obj = new QueryBinder();
            obj.PutOptionsNData = optionsData.Where(x => x.optionType == "PE").OrderByDescending(x => x.strikePrice).ToList();
            //generate tool tip
            obj.PutOptionsNData = GenerateToolTip(obj.PutOptionsNData);
            return obj;
        }

        public QueryBinder GenerateCallData(List<OIDetails> optionsData)
        {
            QueryBinder obj = new QueryBinder();
            obj.CallOptionsNData = optionsData.Where(x => x.optionType == "CE").OrderBy(x => x.strikePrice).ToList();
           var count=obj.CallOptionsNData.Select(x=>x.openInterest);

            //generate tool tip
            obj.CallOptionsNData = GenerateToolTip(obj.CallOptionsNData);

            return obj;
        }
    }
}