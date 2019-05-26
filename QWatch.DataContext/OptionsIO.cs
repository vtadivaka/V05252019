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
    public class OptionsIO
    {
        public float CurrentIndex(float number, float valueRange)
        {
            float div1 = number % valueRange;
            return div1 = div1 >= valueRange / 2 ? number + (valueRange - div1) : number - div1;
        }

        public List<Types> GenerateIndexValues(string actualValue, float valueRange, int requiredSize)
        {
            float actualVal = Convert.ToInt32(Math.Round(Convert.ToDecimal(actualValue),0));
            float currentOptionValue = CurrentIndex(actualVal, valueRange);
            List<Types> objList = new List<Types>();
            Types obj = null;
            obj = new Types();
            obj.Currentvalue = currentOptionValue;
            obj.Type = valueRange;
            List<float> listtypes = new List<float>();
            listtypes.Add(currentOptionValue);
            for (int i = 1; i <= requiredSize; i++)
            {
                listtypes.Add(currentOptionValue + (i * valueRange));
                listtypes.Add(currentOptionValue - (i * valueRange));
            }
            obj.requiredValues = listtypes;
            objList.Add(obj);
            return objList;
        }

        public List<OIDetails> LoadOptionsData(string _masterDatapathBasic, string _bnExpiryDate, string currentIndexValue, float Size,int count)
        {
            OIDetails O = GenerateOIData(currentIndexValue);
            List<OIDetails> objList = new List<OIDetails>();
            List<Types> data = GenerateIndexValues(O.underlyingValue, Size, count);
            string Name = "BANKNIFTY";
            if(Size ==50)
            {
                Name = "NIFTY";
            }

            foreach (var item in data[0].requiredValues)
            {
                if(item.ToString().Contains('.'))
                {
                    string _ceurl = _masterDatapathBasic.Replace("EXPIRYCODE", _bnExpiryDate).Replace("TYPECEPE", "CE").Replace("BANKNIFTY", Name).Replace("INDEXVALUE.00", item.ToString()+"0");
                    objList.Add(GenerateOIData(_ceurl));
                    string _peurl = _masterDatapathBasic.Replace("EXPIRYCODE", _bnExpiryDate).Replace("TYPECEPE", "PE").Replace("BANKNIFTY", Name).Replace("INDEXVALUE.00", item.ToString() + "0");
                    objList.Add(GenerateOIData(_peurl));
                }
                else
                {
                    string _ceurl = _masterDatapathBasic.Replace("EXPIRYCODE", _bnExpiryDate).Replace("TYPECEPE", "CE").Replace("BANKNIFTY", Name).Replace("INDEXVALUE", item.ToString());
                    objList.Add(GenerateOIData(_ceurl));
                    string _peurl = _masterDatapathBasic.Replace("EXPIRYCODE", _bnExpiryDate).Replace("TYPECEPE", "PE").Replace("BANKNIFTY", Name).Replace("INDEXVALUE", item.ToString());
                    objList.Add(GenerateOIData(_peurl));
                }
            }

            return objList;
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
        private static List<string> GetOIProperties()
        {
            List<string> obj = new List<string>();

            obj.Add("lastPrice");
            obj.Add("prevClose");
            obj.Add("openPrice");

            obj.Add("highPrice");
            obj.Add("lowPrice");
            obj.Add("closePrice");

            obj.Add("vwap");
            obj.Add("underlyingValue");
            obj.Add("changeinOpenInterest");

            obj.Add("openInterest");
            obj.Add("pchangeinOpenInterest");
            obj.Add("instrumentType");

            obj.Add("strikePrice");
            obj.Add("expiryDate");
            obj.Add("optionType");
            obj.Add("underlying");
            
            return obj;
        }
        private OIDetails GenerateOIData(string nseurl)
        {
            //string text = System.IO.File.ReadAllText(@"D:\txtData.txt");
            OIDetails obj = new OIDetails();
            string text = GetHtmlString(nseurl);
            try

            {
                string[] text1 = text.Split(new[] { "valid" }, StringSplitOptions.None);
                string[] text2 = text1[1].Split(new[] { "eqLink" }, StringSplitOptions.None);
                string data = text2[0].ToString();
                string s = @""",""";
                s = s.Remove(s.Length - 1);
                string[] text3 = data.Split(new[] { s }, StringSplitOptions.None);

                foreach (var item in GetOIProperties())
                {
                    string result = text3.FirstOrDefault(n => n.Contains(item));
                    string text4 = result.Split(new[] { item }, StringSplitOptions.None)[1].Remove(0, 3);
                    obj.SavedTimeStamp = DateTime.Now;
                    if (item == "lastPrice")
                    {
                        obj.lastPrice = text4;
                    }

                    if (item == "prevClose")
                    {
                        obj.prevClose = text4;
                    }

                    if (item == "openPrice")
                    {
                        obj.openPrice = text4;
                    }

                    if (item == "highPrice")
                    {
                        obj.highPrice = text4.Split('}')[0].Replace("\"", "");
                    }

                    if (item == "lowPrice")
                    {
                        obj.lowPrice = text4;
                    }

                    if (item == "closePrice")
                    {
                        obj.closePrice = text4;
                    }

                    if (item == "vwap")
                    {
                        obj.vwap = text4;
                    }

                    if (item == "underlyingValue")
                    {
                        obj.underlyingValue = text4;
                    }

                    if (item == "changeinOpenInterest")
                    {
                        obj.changeinOpenInterest = text4;
                    }

                    if (item == "prevClose")
                    {
                        obj.prevClose = text4;
                    }
                    if (item == "openInterest")
                    {
                        obj.openInterest = text4;
                    }

                    if (item == "pchangeinOpenInterest")
                    {
                        obj.pchangeinOpenInterest = text4;
                    }

                    if (item == "instrumentType")
                    {
                        obj.instrumentType = text4;
                    }

                    if (item == "strikePrice")
                    {
                        obj.strikePrice = text4;
                    }
                    if (item == "expiryDate")
                    {
                        obj.expiryDate = text4;
                    }

                    if (item == "optionType")
                    {
                        obj.optionType = text4;
                    }
                    if (item == "underlying")
                    {
                        obj.underlying = text4;
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

