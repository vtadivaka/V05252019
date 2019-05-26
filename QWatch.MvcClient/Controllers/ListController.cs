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
    public class ListController : Controller
    {
        public ListController()
        {

        }
        string _masterDatapathOI = ConfigurationManager.AppSettings["MasterDatapathOI"];
        string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
        string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
        //int _resCount =Convert.ToInt32(ConfigurationManager.AppSettings["ResCount"]);
        string _masterDataStaticBankNifty = ConfigurationManager.AppSettings["MasterDataStaticBankNifty"];
        string _masterDataStaticNifty = ConfigurationManager.AppSettings["MasterDataStaticNifty"];

        // GET: List
        public ActionResult Index(string id)
        {
            //Cache7

            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if (true)
            {
                //Cache2
                LogicSaparation obj = new LogicSaparation();
                binder=obj.Index(id, _defaultDate);
            }
             else
            {

            }
            return View(binder);
        }

        public ActionResult AvgAll()
        {
            //Cache7

            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if (true)
            {
                //Cache2
                LogicSaparation obj = new LogicSaparation();
                binder = obj.AvgAll(_defaultDate);
            }
            else
            {

            }
            return View(binder);
        }




        public ActionResult FastMovers()
        {
            //Cache7
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if(true)
            {
                //Cache3
                LogicSaparation obj = new LogicSaparation();
                binder = obj.FastMovers(_defaultDate);
            }
            else
            {

            }
       
            return View(binder);
        }

        public ActionResult TopVolumes()
        {
            //Cache7
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);


            QueryBinder binder = new QueryBinder();
            if (true)
            {    //Cache4
                LogicSaparation obj = new LogicSaparation();
                binder = obj.TopVolumes(_defaultDate);
            }
            else
            {

            }
            return View(binder);
        }


        public ActionResult TodayPicker()
        {
            //Cache7
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if(true)
            {
                //Cache5
                LogicSaparation obj = new LogicSaparation();
                binder = obj.TodayPicker(_defaultDate);
            }
            else
            {

            }
            return View(binder);
        }

        private List<string> GetAvaliableDates( out string displayDate)
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


        public ActionResult RBO()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if(true)
            {
                //Cache6
                LogicSaparation obj = new LogicSaparation();
                binder = obj.RBO(_defaultDate);
            }
            else
            {

            }
            return View(binder);
        }

        public ActionResult DynamicList()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if(true)
            {
                //Cache7
                LogicSaparation obj = new LogicSaparation();
                binder = obj.DynamicList(_defaultDate);
            }
            else
            {

            }
            return View(binder);
        }

        public ActionResult FullDMA()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if (true)
            {
                //To load the weightage data from the file system
                WeightageClass obj1 = new WeightageClass();
                List<Weightage> weightageDataNifty50 = new List<Weightage>();
                weightageDataNifty50 = obj1.WeightageBNData(_masterDataStaticNifty);
                List<Weightage> weightageDataBankNifty = new List<Weightage>();
                weightageDataBankNifty = obj1.WeightageBNData(_masterDataStaticBankNifty);

                LogicSaparation obj = new LogicSaparation();
                binder = obj.FullDMA(weightageDataNifty50, weightageDataBankNifty,_defaultDate);

                ViewBag.NegativeChangeScore = binder.UIDetailedDMA.Where(x => x.ChangeScore < 0).Sum(x => x.ChangeScore);
                ViewBag.PosativeChangeScore = binder.UIDetailedDMA.Where(x => x.ChangeScore > 0).Sum(x => x.ChangeScore);

                ViewBag.NegativeChangeIndexScore = binder.UIDetailedDMA.Where(x => x.IndexScore < 0).Sum(x => x.IndexScore);
                ViewBag.PosativeChangeIndexScore = binder.UIDetailedDMA.Where(x => x.IndexScore > 0).Sum(x => x.IndexScore);
                ViewBag.ChangeIndexScore = binder.UIDetailedDMA.Sum(x => x.IndexScore);

                ViewBag.NiftyScore = binder.NiftyDMAData.Sum(x => x.DMAScore);
                ViewBag.BankNiftyScore = binder.BankNiftyDMAData.Sum(x => x.DMAScore);
            }
            else
            {

            }
            return View(binder);
        }
    }
}