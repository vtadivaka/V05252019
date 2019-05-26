using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using System.IO;
using ObjToXml;
using OptionChainData;
using System.Configuration;

namespace OUI.Controllers
{
    public class OptionChainController : Controller
    {
        public ActionResult LoadData()
        {
            OptionChainClass obj = new OptionChainClass();
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');

            try
            {
                obj.LoadFullOptionChainData(@"D:\Q-Dashboard\MasterData\FullOptionsData\", dates, false);
                Session["optionsData"] = LoadOptionsFullData();
            }
            catch(Exception ex)
            {
                throw new Exception("Error when loading data: ", ex);
            }

            if(System.Web.HttpContext.Current.Request.UrlReferrer ==null || System.Web.HttpContext.Current.Request.UrlReferrer.Segments.Length <3)
            {
                return View();
            }
            else
            {
                return RedirectToAction(System.Web.HttpContext.Current.Request.UrlReferrer.Segments[2]);
            }
        }

        private List<OptionChain> LoadOptionsFullData()
        {
            OBJtoXML xmltoObj = new OBJtoXML();
            string[] txtFiles = Directory.GetFiles(@"D:\Q-Dashboard\MasterData\FullOptionsData\", "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<OptionChain> optionsData = new List<OptionChain>();
            foreach (string currentFile in txtFiles)
            {
                optionsData.AddRange((List<OptionChain>)xmltoObj.CreateOptionsObject(optionsData, currentFile));
            }
            return optionsData;
        }
        public ActionResult CurrentN()
        {
            if(Session["optionsData"] ==null)
            {
                Session["optionsData"] = LoadOptionsFullData();
            }
            List<OptionChain> optionsData = new List<OptionChain>();
            optionsData = (List<OptionChain>)Session["optionsData"];
            optionsData = optionsData.Where(x => x.SourceName == "NIFTY").ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2 = optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            return View(optionsData2);
        }
        public ActionResult CurrentNDay()
        {
            if (Session["optionsData"] == null)
            {
                Session["optionsData"] = LoadOptionsFullData();
            }
            List<OptionChain> optionsData = new List<OptionChain>();
            optionsData = (List<OptionChain>)Session["optionsData"];
            optionsData = optionsData.Where(x => x.SourceName == "NIFTY").ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2 = optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            optionsData2 = GetDateWiseData(optionsData2);
            return View(optionsData2);
        }
        public ActionResult CurrentBNDay()
        {
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');
            if (Session["optionsData"] == null)
            {
                Session["optionsData"] = LoadOptionsFullData();
            }
            List<OptionChain> optionsData = new List<OptionChain>();
            optionsData = (List<OptionChain>)Session["optionsData"];
            optionsData = optionsData.Where(x => x.SourceName == "BANKNIFTY" && x.ExpiryDate == dates[1]).ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2 = optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            optionsData2 = GetDateWiseData(optionsData2);
            return View(optionsData2);
        }
      public ActionResult CurrentBN()
        {
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');
            if (Session["optionsData"] == null)
            {
                Session["optionsData"] = LoadOptionsFullData();
            }
            List<OptionChain> optionsData = new List<OptionChain>();
            optionsData = (List<OptionChain>)Session["optionsData"];
            optionsData = optionsData.Where(x => x.SourceName == "BANKNIFTY" && x.ExpiryDate==dates[1]).ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2= optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            return View(optionsData2);
        }
        private List<OptionChain2> GetDateWiseData(List<OptionChain2> optionsData)
        {
            List<OptionChain2> ObjOptionChain = new List<OptionChain2>();
            string date = string.Empty;

            foreach (var item in optionsData)
            {
                if(item.AsOnTime !=null && item.AsOnTime!="" && date != item.AsOnTime.Remove(item.AsOnTime.Length - 9).Trim())
                {
                    date = item.AsOnTime.Remove(item.AsOnTime.Length-9).Trim();
                    ObjOptionChain.Add(item);
                }
            }
            return ObjOptionChain;
        }
        private List<OptionChain2> GetPersonalisedoptionsData(List<OptionChain> optionsData)
        {
            List<OptionChain2> ObjOptionChain = new List<OptionChain2>();
            OptionChain2 objChain = null;

            string tempAsonDate = string.Empty;

            if (optionsData.Count>1)
            {
                for (int i = 1; i < optionsData.Count; i++)
                {
                    objChain = new OptionChain2();

                    
                     if (tempAsonDate != optionsData[i].AsOnTime || tempAsonDate==null)
                    {
                        objChain.ExpiryDate = optionsData[i].ExpiryDate;
                        objChain.Percentage = optionsData[i].Percentage;
                        objChain.SourceName = optionsData[i].SourceName;
                        objChain.SourceType = optionsData[i].SourceType;
                        objChain.SourceValue = optionsData[i].SourceValue;
                        objChain.Strikes = optionsData[i].Strikes;
                        objChain.TimeStamp = optionsData[i].TimeStamp;
                        objChain.UnderlyingValue = optionsData[i].UnderlyingValue;
                        objChain.AsOnTime = optionsData[i].AsOnTime;
                        tempAsonDate = objChain.AsOnTime;

                        objChain.total_cal_ChnginOI = optionsData[i].total_cal_ChnginOI + "(" + (optionsData[i].total_cal_ChnginOI - optionsData[i - 1].total_cal_ChnginOI) + ")";
                        objChain.total_cal_OI = optionsData[i].total_cal_OI + "(" + (optionsData[i].total_cal_OI - optionsData[i - 1].total_cal_OI) + ")";
                        objChain.total_cal_Volume = optionsData[i].total_cal_Volume + "(" + (optionsData[i].total_cal_Volume - optionsData[i - 1].total_cal_Volume) + ")";
                        objChain.total_put_ChnginOI = optionsData[i].total_put_ChnginOI + "(" + (optionsData[i].total_put_ChnginOI - optionsData[i - 1].total_put_ChnginOI) + ")";
                        objChain.total_put_OI = optionsData[i].total_put_OI + "(" + (optionsData[i].total_put_OI - optionsData[i - 1].total_put_OI) + ")";
                        objChain.total_put_Volume = optionsData[i].total_put_Volume + "(" + (optionsData[i].total_put_Volume - optionsData[i - 1].total_put_Volume) + ")";
                        ObjOptionChain.Add(objChain);
                    }
                }
            }
            return ObjOptionChain;
        }

        public ActionResult FinalBN()
        {
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');

            if (Session["optionsData"] == null)
            {
                Session["optionsData"] = LoadOptionsFullData();
            }
            List<OptionChain> optionsData = new List<OptionChain>();
            optionsData = (List<OptionChain>)Session["optionsData"];
            optionsData = optionsData.Where(x => x.SourceName == "BANKNIFTY" && x.ExpiryDate == dates[0]).ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2 = optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            return View(optionsData2);
        }
    }
}