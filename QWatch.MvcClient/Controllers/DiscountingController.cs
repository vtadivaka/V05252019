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
    public class DiscountingController : Controller
    {
        string _masterDatapathOI = ConfigurationManager.AppSettings["MasterDatapathOI"];
        string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
        string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
        //int _resCount =Convert.ToInt32(ConfigurationManager.AppSettings["ResCount"]);
        string _masterDataStaticBankNifty = ConfigurationManager.AppSettings["MasterDataStaticBankNifty"];
        string _masterDataStaticNifty = ConfigurationManager.AppSettings["MasterDataStaticNifty"];
        // GET: Discounting
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Discounting()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);

            QueryBinder binder = new QueryBinder();
            if (true)
            {
                //Cache2
                LogicSaparation obj = new LogicSaparation();
                binder = obj.Discounting("Discounting", _defaultDate);
                List<DiscountEntity> discData= GenerateDiscountData(binder.CheckPriceStrongOIStroing, binder.PosativeData);
                binder.DiscountData = GenerateSmartViewData(discData).Where(x=>x.ExpectedChange !="F" && x.ExpectedChange != "T").ToList();
            }
            else
            {

            }
            return View(binder);
        }

        private List<DiscountEntity> GenerateSmartViewData(List<DiscountEntity> discData)
        {
            List<DiscountEntity> discountList = new List<DiscountEntity>();
            foreach (var item in discData)
            {
                item.IsBasicMaxValuePreviousClose = false;
                item.IsBasicMaxValueClose = false;

                if (Convert.ToDecimal(item.BasicPreviousClose) > Convert.ToDecimal(item.IOPreviousClose))
                {
                    item.IsBasicMaxValuePreviousClose = true;
                }


                if (item.IOClosed == null || item.IOClosed == "0.00" || item.BasicClosed == null || item.BasicClosed == "0.00")
                {
                    if (Convert.ToDecimal(item.BasicLtp) > Convert.ToDecimal(item.IOLtp))
                    {
                        item.IsBasicMaxValueClose = true;
                    }
                }
                else
                {
                    if (Convert.ToDecimal(item.BasicClosed) > Convert.ToDecimal(item.IOClosed))
                    {
                        item.IsBasicMaxValueClose = true;
                    }
                }

                if(item.IsBasicMaxValuePreviousClose != item.IsBasicMaxValueClose)
                {
                    if (item.IOClosed == null || item.IOClosed == "0.00" || item.BasicClosed == null || item.BasicClosed == "0.00")
                    {
                        if (Convert.ToDecimal(item.BasicLtp) > Convert.ToDecimal(item.IOLtp))
                        {
                            item.ExpectedChange = "Negative";
                        }
                        else
                        {
                            item.ExpectedChange = "Posative";
                        }
                    }
                    else
                    {
                        if (Convert.ToDecimal(item.BasicClosed) > Convert.ToDecimal(item.IOClosed))
                        {
                            item.ExpectedChange = "Negative";
                        }
                        else
                        {
                            item.ExpectedChange = "Posative";
                        }
                    }
                   
                }
                else
                {
                    if(item.IsBasicMaxValuePreviousClose)
                    {
                        item.ExpectedChange = "T";
                    }
                    else
                    {
                        item.ExpectedChange = "F";
                    }

                }
                discountList.Add(item);
            }
            return discountList;
        }




        private List<DiscountEntity> GenerateDiscountData(List<OIDetails> OIdata, List<CompanyDetails> Basicdata)
        {
            List<DiscountEntity> discountList = new List<DiscountEntity>();
            DiscountEntity objitem = null;
            foreach (var oIItem in OIdata.Where(x=>x.CompanyCode !=null))
            {
                objitem = new DiscountEntity();
                var basicItem = Basicdata.Where(x => x.Code == oIItem.CompanyCode).ToList();
                if (basicItem.Count > 0)
                {
                    objitem.CompanyCode = oIItem.CompanyCode;

                    objitem.IOLtp = oIItem.LastPrice;
                    objitem.IOPreviousClose = oIItem.PrevClose;
                    objitem.IOOpenPrice = oIItem.OpenPrice;
                    objitem.IOLowPrice = oIItem.LowPrice;
                    objitem.IOHighPrice = (oIItem.HighPrice.Split('}'))[0].Replace("\"", "");
                    objitem.IOPercentChange = oIItem.PChange;
                    objitem.IOClosed = oIItem.Closed;


                    objitem.BasicLtp = basicItem[0].lastPrice;
                    objitem.BasicPreviousClose = basicItem[0].PreClose;
                    objitem.BasicOpenPrice = basicItem[0].Open;
                    objitem.BasicLowPrice = basicItem[0].Low;
                    objitem.BasicHighPrice = basicItem[0].High;
                    objitem.BasicPercentChange = basicItem[0].pChange;
                    objitem.BasicClosed = basicItem[0].Closed;

                    Decimal DiscountPrice = 0;

                    if (objitem.IOPercentChange !="-" && objitem.BasicPercentChange != "-")
                    {
                        if (objitem.IOClosed !=null && objitem.IOClosed != "" && objitem.BasicClosed != null && objitem.BasicClosed != "")
                        {
                            DiscountPrice=((Convert.ToDecimal(objitem.IOClosed) - Convert.ToDecimal(objitem.BasicClosed)) / Math.Abs(Convert.ToDecimal(objitem.BasicClosed))) * 100;
                            objitem.DiscountPrice =Convert.ToString(Math.Round(DiscountPrice, 2));
                        }
                        else if(objitem.IOLtp != null && objitem.IOLtp != "" && objitem.BasicLtp != null && objitem.BasicLtp != "")
                        {
                            DiscountPrice = ((Convert.ToDecimal(objitem.IOLtp) - Convert.ToDecimal(objitem.BasicLtp)) / Math.Abs(Convert.ToDecimal(objitem.BasicLtp))) * 100;
                            objitem.DiscountPrice = Convert.ToString(Math.Round(DiscountPrice, 2));
                        } 
                    }
                    discountList.Add(objitem);
                }
            }
            return discountList.OrderByDescending(x=> Convert.ToDecimal(x.DiscountPrice)).ToList();
        }

        private List<string> GetAvaliableDates(out string displayDate)
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