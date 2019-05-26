using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using Entity;
using ObjToXml;
using DataContext;
using QuickWatchMethods;


namespace MvcClient.Controllers
{
    public class MCController : Controller
    {
        string _masterDatapathOI = ConfigurationManager.AppSettings["MasterDatapathOI"];
        string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
        string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
        //int _resCount =Convert.ToInt32(ConfigurationManager.AppSettings["ResCount"]);
        string _masterDataStaticBankNifty = ConfigurationManager.AppSettings["MasterDataStaticBankNifty"];
        string _masterDataStaticNifty = ConfigurationManager.AppSettings["MasterDataStaticNifty"];
        public ActionResult MCData()
        {

            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);
            QueryBinder binder = new QueryBinder();
            LogicSaparation obj = new LogicSaparation();
            binder = obj.MCData(_defaultDate);
            return View(binder);
        }
        public List<string> GetAvaliableDates(out string displayDate)
        {
            LogicSaparation objLogicSaparation = new LogicSaparation();
            List<string> dates = objLogicSaparation.DateCollection();
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
    }
}