using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using Entity;

namespace DataContext
{
    public class IOData
    {
        public List<OIDetails> LoadOIData(string _nseUrl, string _expiryDate, string _listofSources)
        {
            IOData objData = new IOData();
            string[] listsources = _listofSources.Split(',');
            List<OIDetails> objList = new List<OIDetails>();
            List<string> dnames = new List<string>();

            foreach (var item in listsources)
            {
                try
                {
                    string[] names = item.Split('#');
                    string newNseUrl = _nseUrl.Replace("NSECOMPANYNAME", names[0]);
                    newNseUrl = newNseUrl.Replace("INDEXTYPE", names[2]);
                    newNseUrl = newNseUrl.Replace("EXPIRYDATE", _expiryDate);
                    OIDetails data= objData.GenerateOIData(newNseUrl);
                    objList.Add(data);

                    
                   if (data.pchangeinOpenInterest == null || data.pchangeinOpenInterest == "")
                      {
                           dnames.Add(newNseUrl);
                      }
                }
                catch (Exception ex)
                {

                }
            }
            return objList.Where(x=>x.CompanyCode !=null).ToList();
        }

        private static string GetHtmlString(string url)
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
        private static List<string> GetOIProperties()
        {
            List<string> obj = new List<string>();

            obj.Add("underlying");
            obj.Add("highPrice");
            obj.Add("instrumentType");
            obj.Add("settlementPrice");
            obj.Add("pchangeinOpenInterest");
            obj.Add("expiryDate");
            obj.Add("marketLot");
            obj.Add("changeinOpenInterest");
            obj.Add("openInterest");
            obj.Add("underlyingValue");
            obj.Add("numberOfContractsTraded");
            obj.Add("lowPrice");
            obj.Add("lastPrice");
            obj.Add("lastUpdateTime");
            obj.Add("prevClose");
            obj.Add("pChange");
            obj.Add("totalSellQuantity");
            obj.Add("totalBuyQuantity");
            obj.Add("openPrice");
            obj.Add("closePrice");
            
            return obj;
        }
        private OIDetails GenerateOIData(string nseurl)
        {
            //string text = System.IO.File.ReadAllText(@"D:\txtData.txt");
            OIDetails obj = new OIDetails();
            string text = GetHtmlString(nseurl);
            try
            {
                string[] text1 = text.Split(new[] { "content_big" }, StringSplitOptions.None);
                string[] text2 = text1[0].Split(new[] { "tradedDate" }, StringSplitOptions.None);
                string data = text2[1].ToString();
                string s = @""",""";
                s = s.Remove(s.Length - 1);
                string[] text3 = data.Split(new[] { s }, StringSplitOptions.None);

                foreach (var item in GetOIProperties())
                {
                    string result = text3.FirstOrDefault(n => n.Contains(item));
                    result = result.Split(new[] { "</div>" }, StringSplitOptions.None)[0].Trim();
                    string text4 = result.Split(new[] { item }, StringSplitOptions.None)[1].Remove(0, 3);

                    obj.OISavedTimeStamp = DateTime.Now;
                    if (item == "underlying")
                    {
                        obj.CompanyCode = text4;
                    }

                    if (item == "highPrice")
                    {
                        obj.HighPrice = text4;
                    }

                    if (item == "pchangeinOpenInterest")
                    {
                        obj.PchangeinOpenInterest = text4;
                    }

                    if (item == "expiryDate")
                    {
                        obj.ExpiryDate = text4;
                    }

                    if (item == "openInterest")
                    {
                        obj.OpenInterest = text4;
                    }

                    if (item == "underlyingValue")
                    {
                        obj.UnderlyingValue = text4;
                    }

                    if (item == "numberOfContractsTraded")
                    {
                        obj.NumberOfContractsTraded = text4;
                    }

                    if (item == "lowPrice")
                    {
                        obj.LowPrice = text4;
                    }

                    if (item == "lastPrice")
                    {
                        obj.LastPrice = text4.Split('}')[0].Remove(text4.Split('}')[0].Length - 1);
                    }

                    if (item == "prevClose")
                    {
                        obj.PrevClose = text4;
                    }
                    if (item == "lastUpdateTime")
                    {
                        obj.LastUpdateTime = text4;
                    }

                    if (item == "pChange")
                    {
                        obj.PChange = text4;
                    }

                    if (item == "totalBuyQuantity")
                    {
                        obj.totalBuyQuantity = text4;
                    }

                    if (item == "totalSellQuantity")
                    {
                        obj.totalSellQuantity = text4;
                    }

                    if (item == "openPrice")
                    {
                        obj.OpenPrice = text4;
                    }

                    if (item == "closePrice")
                    {
                        obj.Closed = text4;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
    }
}
