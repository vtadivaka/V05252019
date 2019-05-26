using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using Entity;
using System.IO;

namespace DataContext
{
    public static class BasicData
    {
        public static List<CompanyDetails> ColletData(string _nseUrl, string _nseBseList)
        {
            string[] listsources = _nseBseList.Split(',');
            List<CompanyDetails> objList = new List<CompanyDetails>();
            foreach (var item in listsources)
            {
                string[] names = item.Split('#');
                string newNseUrl = _nseUrl.Replace("NSECOMPANYNAME", names[0]);
                objList.Add(GenerateData(newNseUrl));
            }
            return objList.Where(x=>x.Code !=null).ToList();
        }

        public static CompanyDetails GenerateData(string nseurl)
        {
            // string text = System.IO.File.ReadAllText(@"D:\txtData.txt");
            CompanyDetails obj = new CompanyDetails();
            string text = GetHtmlString(nseurl);
            try

            {
                string[] text1 = text.Split(new[] { "content_big" }, StringSplitOptions.None);
                string[] text2 = text1[0].Split(new[] { "tradedDate" }, StringSplitOptions.None);
                string data = text2[1].ToString();
                string s = @""",""";
                s = s.Remove(s.Length - 1);
                string[] text3 = data.Split(new[] { s }, StringSplitOptions.None);

                foreach (var item in GetProperties())
                {
                    string result = text3.FirstOrDefault(n => n.Contains(item));
                     result = result.Split(new[] { "</div>" }, StringSplitOptions.None)[0].Trim();
                    string text4 = result.Split(new[] { item }, StringSplitOptions.None)[1].Remove(0, 3);


                    obj.BasicSavedTimeStamp = DateTime.Now;
                    if (item == "lastUpdateTime")
                    {
                        obj.Date = text4.Remove(text4.Length - 2);
                    }

                    if (item == "companyName")
                    {
                        obj.CompanyName = text4;
                    }

                    if (item == "previousClose")
                    {
                        obj.PreClose = text4;
                    }

                    if (item == "open")
                    {
                        obj.Open = text4;
                    }

                    if (item == "dayHigh")
                    {
                        obj.High = text4;
                    }

                    if (item == "dayLow")
                    {
                        obj.Low = text4;
                    }

                    if (item == "closePrice")
                    {
                        obj.Closed = text4;
                    }

                    if (item == "pChange")
                    {
                        obj.pChange = text4;
                    }

                    if (item == "change")
                    {
                        obj.change = text4;
                    }

                    if (item == "low52")
                    {
                        obj.low52 = text4;
                    }

                    if (item == "high52")
                    {
                        obj.high52 = text4;
                    }

                    if (item == "deliveryQuantity")
                    {
                        obj.DeliveryVolume = text4;
                    }

                    if (item == "totalTradedVolume")
                    {
                        obj.TotalVolume = text4;
                    }

                    if (item == "deliveryToTradedQuantity")
                    {
                        obj.Delivery = text4;
                    }
                    if (item == "symbol")
                    {
                        obj.Code = text4;
                    }
                    if (item == "totalSellQuantity")
                    {
                        if(text4=="-")
                        {
                            text4 = "1";
                        }
                        obj.totalSellQuantity = text4;
                    }
                    if (item == "totalBuyQuantity")
                    {
                        if (text4 == "-")
                        {
                            text4 = "1";
                        }
                        obj.totalBuyQuantity = text4;
                    }
                    if (item == "lastPrice")
                    {
                        obj.lastPrice = text4.Split('}')[0].Remove(text4.Split('}')[0].Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public static string GetHtmlString(string url)
        {
            string sHtml = "";
            HttpWebRequest request;
            HttpWebResponse response = null;
            Stream stream = null;


            request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Foo";
            request.Accept = "*/*";
            response = (HttpWebResponse)request.GetResponse();
            stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
            sHtml = sr.ReadToEnd();
            if (stream != null) stream.Close();
            if (response != null) response.Close();
            return sHtml;
        }
        private static List<CompanyDetails> GenerateData()
        {
            List<CompanyDetails> objlist = new List<CompanyDetails>();
            CompanyDetails obj1 = new CompanyDetails();
            obj1.CompanyName = "CompanyName";
            obj1.Code = "Code";
            obj1.PreClose = "PreClose";
            obj1.Open = "Open";
            obj1.High = "High";
            obj1.Low = "Low";
            obj1.Closed = "Closed";
            obj1.TotalVolume = "TotalVolume";
            obj1.DeliveryVolume = "DeliveryVolume";
            obj1.Delivery = "Delivery";


            obj1.pChange = "pChange";
            obj1.change = "pChange";
            obj1.high52 = "pChange";
            obj1.low52 = "pChange";

            DateTime time = DateTime.Now;       // Use current time.
            string format = "d MMM yyyy";      // Use this format.
            string datetime = time.ToString(format);

            obj1.Date = datetime;
            obj1.UpdateToday = "The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.";

            objlist.Add(obj1);


            obj1.CompanyName = "CompanyName1";
            obj1.Code = "Code1";
            obj1.PreClose = "PreClose1";
            obj1.Open = "Open1";
            obj1.High = "High1";
            obj1.Low = "Low1";
            obj1.Closed = "Closed1";
            obj1.TotalVolume = "TotalVolume1";
            obj1.DeliveryVolume = "DeliveryVolume1";
            obj1.Delivery = "Delivery1";
            obj1.pChange = "pChange1";
            obj1.change = "pChange1";
            obj1.high52 = "pChange1";
            obj1.low52 = "pChange1";
            DateTime time1 = DateTime.Now;       // Use current time.
            string format1 = "d MMM yyyy";      // Use this format.
            string datetime1 = time1.ToString(format);

            obj1.Date = datetime1;
            obj1.UpdateToday = "The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.";

            objlist.Add(obj1);

            obj1.CompanyName = "CompanyName2";
            obj1.Code = "Code3";
            obj1.PreClose = "PreClose3";
            obj1.Open = "Open2";
            obj1.High = "High2";
            obj1.Low = "Low2";
            obj1.Closed = "Closed2";
            obj1.TotalVolume = "TotalVolume2";
            obj1.DeliveryVolume = "DeliveryVolume2";
            obj1.Delivery = "Delivery2";
            obj1.pChange = "pChange2";
            obj1.change = "pChange2";
            obj1.high52 = "pChange2";
            obj1.low52 = "pChange2";
            DateTime time2 = DateTime.Now;       // Use current time.
            string format2 = "d MMM yyyy";      // Use this format.
            string datetime2 = time2.ToString(format);

            obj1.Date = datetime2;
            obj1.UpdateToday = "The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.";

            objlist.Add(obj1);


            return objlist;
        }

        private static List<string> GetProperties()
        {
            List<string> obj = new List<string>();

            obj.Add("lastUpdateTime");
            obj.Add("companyName");
            obj.Add("previousClose");
            obj.Add("open");
            obj.Add("dayHigh");
            obj.Add("dayLow");
            obj.Add("closePrice");
            obj.Add("pChange");
            obj.Add("change");
            obj.Add("low52");
            obj.Add("high52");
            obj.Add("deliveryQuantity");
            obj.Add("quantityTraded");
            obj.Add("deliveryToTradedQuantity");
            obj.Add("symbol");
            obj.Add("totalBuyQuantity");
            obj.Add("totalSellQuantity");
            obj.Add("lastPrice");
            obj.Add("totalTradedVolume");

            return obj;
        }
    }
}


