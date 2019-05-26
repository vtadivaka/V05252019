using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OIOData;
using Entity;
using System.Configuration;
using ObjToXml;

namespace OptionChainData
{
    public class OptionChainClass
    {
        public void LoadFullOptionChainData(string filepathtoSave, string[] dates, bool isLoadAll)
        {
            List<OptionChain> chainData = new List<OptionChain>();
            string _masterDataStaticBankNifty = ConfigurationManager.AppSettings["MasterDataStaticBankNifty"];
            string _masterDataStaticNifty = ConfigurationManager.AppSettings["MasterDataStaticNifty"];
           // List<SourceList> listofSources = StrikeSource.Sources(_masterDataStaticBankNifty, _masterDataStaticNifty, "25OCT2018", "11OCT2018");
            List<SourceList> listofSources = StrikeSource.Sources(_masterDataStaticBankNifty, _masterDataStaticNifty, dates[0], dates[1], isLoadAll);

            foreach (var item in listofSources)
            {
                string url = "https://www.nseindia.com/live_market/dynaContent/live_watch/option_chain/optionKeys.jsp?segmentLink=17&instrument=OPTIDX&symbol=BANKNIFTY&date=XXXDATE";
                url = url.Replace("OPTIDX", item.instrument).Replace("BANKNIFTY", item.symbol).Replace("XXXDATE", item.date);
                chainData.Add(GenerateOptionChain(url, item.SourceName, item.date, item.instrument));
            }

            OBJtoXML xmltoObj = new OBJtoXML();
            xmltoObj.CreateOptionsXML(chainData, filepathtoSave, "LoadFullOptionChainData");
        }
        
        private List<Strikes> GenerateStrikes(List<string> textX)
        {
            textX.RemoveAt(textX.Count() - 1);
            List<Strikes> list = new List<Strikes>();
            Strikes objItem = null;
            foreach (var item in textX)
            {
                string[] item2 = item.Split(new[] { "<td" }, StringSplitOptions.None);
                List<string> textY = item2.ToList();
                textY.RemoveAt(0);
                textY.RemoveAt(textY.Count - 1);
                objItem = new Strikes();
                for (int i = 0; i <= textY.Count - 1; i++)
                {

                    objItem.Time = DateTime.Now.ToString();
                    if (i == 0)
                    {
                        objItem.Cal_OI = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 1)
                    {
                        objItem.Cal_ChnginOI = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 2)
                    {
                        objItem.Cal_Volume = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 3)
                    {
                        objItem.Cal_IV = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 4)
                    {
                        objItem.Cal_LTP = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 5)
                    {
                        objItem.Cal_NetChng = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 6)
                    {
                        objItem.Cal_BidQty = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 7)
                    {
                        objItem.Cal_BidPrice = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 8)
                    {
                        objItem.Cal_AskPrice = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 9)
                    {
                        objItem.Cal_AskQty = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }


                    else if (i == 10)
                    {
                        objItem.StrikePrice = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[3].Split('<'))[0];
                    }



                    else if (i == 11)
                    {
                        objItem.Put_BidQty = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 12)
                    {
                        objItem.Put_BidPrice = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 13)
                    {
                        objItem.Put_AskPrice = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 14)
                    {
                        objItem.Put_AskQty = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 15)
                    {
                        objItem.Put_NetChng = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 16)
                    {
                        objItem.Put_LTP = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 17)
                    {
                        objItem.Put_IV = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];

                    }
                    else if (i == 18)
                    {
                        objItem.Put_Volume = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 19)
                    {
                        objItem.Put_ChnginOI = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                    else if (i == 20)
                    {
                        objItem.Put_OI = ((textY[i].Split('>'))[1].Trim().Split('<'))[0] != string.Empty ? ((textY[i].Split('>'))[1].Split('<'))[0] : ((textY[i].Split('>'))[2].Split('<'))[0];
                    }
                }
                list.Add(objItem);
            }

            return list;
        }

        private OptionChain GenerateFinalData(string textForChain)
        {
            OptionChain obj = new OptionChain();
            string[] text1 = textForChain.Split(new[] { "</b></td>" }, StringSplitOptions.None);
            int j = 0;
            for (int i = 0; i <= text1.Count() - 1; i++)
            {
                string[] text2 = text1[i].Split(new[] { "<b>" }, StringSplitOptions.None);
                if (j == 0 && text2[1] != "Total" && text2[1] != null && text2[1] != string.Empty)
                {
                    obj.total_cal_OI = Convert.ToInt64(text2[1].Replace(",", "").Trim()); j++;
                }
                else if (j == 1 && text2[1] != "Total" && text2[1] != null && text2[1] != string.Empty)
                {
                    obj.total_cal_Volume = Convert.ToInt64(text2[1].Replace(",", "").Trim()); j++;
                }
                else if (j == 2 && text2[1] != "Total" && text2[1] != null && text2[1] != string.Empty)
                {
                    obj.total_put_Volume = Convert.ToInt64(text2[1].Replace(",", "").Trim()); j++;
                }
                else if (j == 3 && text2[1] != "Total" && text2[1] != null && text2[1] != string.Empty)
                {
                    obj.total_put_OI = Convert.ToInt64(text2[1].Replace(",", "").Trim()); j++;
                }
            }
            return obj;
        }
        private string GetUnderlyingValue(string newdata)
        {
            string[] text1 = newdata.Split(new[] { "Underlying Index:" }, StringSplitOptions.None);
            string[] text2 = text1[1].Split(new[] { "</b>" }, StringSplitOptions.None);
            string[] text3 = text2[0].Split(new[] { " " }, StringSplitOptions.None);
            string value = text3[text3.Length - 1];
            return value;
        }

        private string GetAsOnTime(string newdata)
        {
            string[] text1 = newdata.Split(new[] { "<span>As on" }, StringSplitOptions.None);
            string[] text2 = text1[1].Split(new[] { "IST" }, StringSplitOptions.None);
            string value = text2[0];
            return value;
        }

        private OptionChain GenerateOptionChain(string url, string sourceName, string date, string instment)
        {
            OptionChain objChain = new OptionChain();

            string data = OIOData.OptionsIO.GetHtmlString(url);
            

            string[] text1 = data.Split(new[] { "javascript:chartPopup" }, StringSplitOptions.None);
            List<string> textX = text1.ToList();
            textX.RemoveAt(0);
            string textForChain = textX[textX.Count() - 1];
            objChain = GenerateFinalData(textForChain);
            List<Strikes> sourceStrickes = GenerateStrikes(textX);
            List<Strikes> listData = sourceStrickes.Where(x => x.StrikePrice != null && x.StrikePrice != string.Empty).ToList();


            objChain.SourceName = sourceName;
            objChain.ExpiryDate = date;
            objChain.SourceType = date;
            objChain.SourceValue = "";


            var totalData1 = listData.Where(x => x.Cal_ChnginOI != null && x.Cal_ChnginOI != "" && x.Cal_ChnginOI != "-").ToList();
            objChain.total_cal_ChnginOI = totalData1.Sum(x => Convert.ToInt64(x.Cal_ChnginOI.Replace(",", "").Trim()));

            var totalData2 = listData.Where(x => x.Put_ChnginOI != null && x.Put_ChnginOI != "" && x.Put_ChnginOI != "-").ToList();
            objChain.total_put_ChnginOI = totalData2.Sum(x => Convert.ToInt64(x.Put_ChnginOI.Replace(",", "").Trim()));

            long localPercentage = objChain.total_put_OI - objChain.total_cal_OI;
            decimal localPercentage2 = (localPercentage * 100 / objChain.total_put_OI);
            objChain.Percentage = localPercentage2.ToString();

            objChain.TimeStamp = DateTime.Now.ToString();
            objChain.Strikes = listData;

            objChain.UnderlyingValue = GetUnderlyingValue(data);
            objChain.AsOnTime= GetAsOnTime(data);
            return objChain;
        }
    }
}
