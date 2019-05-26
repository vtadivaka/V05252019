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
    public class AdminController : Controller
    {
        string _masterDatapathOI = ConfigurationManager.AppSettings["MasterDatapathOI"];
        string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
        string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
        int _resCount = Convert.ToInt32(ConfigurationManager.AppSettings["ResCount"]);
        string _userNotes = ConfigurationManager.AppSettings["UserNotes"];

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


        // GET: Admin
        public ActionResult RefreshPostData()
        {
            string _defaultDate = string.Empty;
            List<string> dates = GetAvaliableDates(out _defaultDate);
            GenerateBackDateURLs(dates);
            return View();
        }
        public ActionResult UserNotes()
        {
            return View(LoadData(System.IO.File.ReadAllLines(_userNotes)));
        }
        [HttpPost]
        public ActionResult UserNotes(List<UserData> Data)
        {
            int count = System.IO.File.ReadAllLines(_userNotes).Length;
            using (StreamWriter sw = new StreamWriter(_userNotes, true))
            {
                sw.WriteLine(string.Concat(count + ",",DateTime.Now.ToString("MM/dd/yyyy")  + ",", Data[0].Description + ","));
                sw.Close();
                sw.Dispose();
            }
            return View(LoadData(System.IO.File.ReadAllLines(_userNotes)));
        }

        private List<UserData> LoadData(string[] text)
        {
            List<UserData> objData = new List<UserData>();
           UserData user;
            int i = 0;
            foreach (var item in text)
            {
                user = new UserData();
                if (i!=0)
                {
                    string[] data = item.Split(',');
                    user.Id = Convert.ToInt32(data[0]);
                    DateTime oDate = Convert.ToDateTime(data[1]);
                    user.IssueDate = oDate;

                    if (data.Length>3)
                    {
                        int j = 0;
                        foreach (var dataitem in data)
                        {
                            if(j==0 || j==1)
                            {
                                j = j + 1;
                            }
                            else
                            {
                                user.Description = user.Description + dataitem;
                            }
                        }
                    }
                    else
                    {
                        user.Description = data[2];
                    }
                    
                }
                else
                {
                    i = i+1;
                }

                objData.Add(user);
            }

            return objData.OrderByDescending(x=>x.Id).ToList();
        }
    }
}