using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjToXml;
using Entity;
using DataContext;
using System.IO;
using System.Collections;

namespace QuickWatchMethods
{
   public class DMALogic
    {

        private List<string> FilterDates(List<string> files, string _defaultDate)
        {
            List<string> list = new List<string>();
            bool value = false;
            foreach (var item in files)
            {
                if(value ==true)
                {
                    list.Add(item);
                }
                else
                {
                    if (item.Trim().ToLower().Contains(_defaultDate.Trim().ToLower()))
                    {
                        value = true;
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        public List<Rank> CheckLTPHasFirstPlace(string _masterDatapathDMA, string _defaultDate)
        {
            //test to read data
            BulkEntity datafromXML = new BulkEntity();
            OBJtoXML xmltoObj = new OBJtoXML();
            datafromXML.DMAData = new List<Rank>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathDMA, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false));break;
            }
            List<Rank> dataitem1 = datafromXML.DMAData.Where(x => x.RankId == 1 && x.Day == "LTP").ToList();
            return dataitem1;
        }
        public List<Rank> CheckLTPHasSecondPlace(string _masterDatapathDMA, string _defaultDate)
        {
            //test to read data
            BulkEntity datafromXML = new BulkEntity();
            OBJtoXML xmltoObj = new OBJtoXML();
            datafromXML.DMAData = new List<Rank>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathDMA, "*.txt").OrderByDescending(d => new FileInfo(d).CreationTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false)); break;
            }
            List<Rank> dataitem1 = datafromXML.DMAData.Where(x => x.RankId == 2 && x.Day == "LTP").ToList();
            return dataitem1;
        }

        public List<Rank> CheckLTPHasLastPlace(string _masterDatapathDMA, string _defaultDate)
        {
            //test to read data
            BulkEntity datafromXML = new BulkEntity();
            OBJtoXML xmltoObj = new OBJtoXML();
            datafromXML.DMAData = new List<Rank>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathDMA, "*.txt").OrderByDescending(d => new FileInfo(d).CreationTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false)); break;
            }
            List<Rank> dataitem1 = datafromXML.DMAData.Where(x => x.RankId == 8 && x.Day == "LTP").ToList();
            return dataitem1;
        }
        public List<Rank> CheckLTPHasBeforeLastPlace(string _masterDatapathDMA, string _defaultDate)
        {
            //test to read data
            BulkEntity datafromXML = new BulkEntity();
            OBJtoXML xmltoObj = new OBJtoXML();
            datafromXML.DMAData = new List<Rank>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathDMA, "*.txt").OrderByDescending(d => new FileInfo(d).CreationTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false)); break;
            }
            List<Rank> dataitem1 = datafromXML.DMAData.Where(x => x.RankId == 7 && x.Day == "LTP").ToList();
            return dataitem1;
        }
        public List<Rank> BankniftyDMAScore(string _masterDatapathDMA, List<Weightage> bankNifty, string _defaultDate)
        {
            //test to read data
            BulkEntity datafromXML = new BulkEntity();
            OBJtoXML xmltoObj = new OBJtoXML();
            datafromXML.DMAData = new List<Rank>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathDMA, "*.txt").OrderByDescending(d => new FileInfo(d).CreationTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false)); break;
            }
            List<Rank> dataitem1 = GenerateDMAScore(datafromXML.DMAData.ToList(), bankNifty);
            return dataitem1;
        }
            
        private List<Rank> GenerateDMAScore(List<Rank> dataitem1, List<Weightage> bankNifty)
        {
            ConvertiontoDMADayData objNewListObj = new ConvertiontoDMADayData();
            List<UIDetailedDMA> objNewListData = objNewListObj.ConvertOnjectToDMAData(dataitem1).ToList();

            List<Rank> newList = new List<Rank>();
            foreach (var item in bankNifty)
            {
                List<UIDetailedDMA> nList = objNewListData.Where(x => x.SourceName.Trim().ToUpper() == item.Name.Trim().ToUpper()).ToList();
                if(nList.Count ==1)
                {
                    decimal score = CalculateScore(nList[0].UniqueString, item.Value);
                    Rank rankObject = new Rank();
                    rankObject.SourceName = nList[0].SourceName;
                    rankObject.DMAScore = score;
                    newList.Add(rankObject);
                }
            }
            return newList;
        }


        private decimal CalculateScore(string dataString, string value)
        {
            int score = 0;
            string[] data = dataString.Split('-');
            bool ltpCame = false;
            foreach (var item in data)
            {
                if(item !=null && item != "")
                {
                    if(item == "LTP")
                    {
                        ltpCame = true;
                    }
                    else if(item == "ThirtyDMA")
                    {
                        if(ltpCame==true)
                        {
                            score = score + 2;
                        }
                        else
                        {
                            score = score - 2;
                        }
                    }
                    else if (item == "FiftyDMA")
                    {
                        if (ltpCame == true)
                        {
                            score = score + 3;
                        }
                        else
                        {
                            score = score - 3;
                        }
                    }
                    else if (item == "HundredDMA")
                    {
                        if (ltpCame == true)
                        {
                            score = score + 2;
                        }
                        else
                        {
                            score = score - 2;
                        }
                    }
                    else if (item == "TwoHundredDMA")
                    {
                        if (ltpCame == true)
                        {
                            score = score + 3;
                        }
                        else
                        {
                            score = score - 3;
                        }
                    }
                }
            }
            return Convert.ToDecimal(score) * Convert.ToDecimal(value);
        }
        public List<UIDetailedDMA> ChangeDMAPosation(string _masterDatapathDMA, List<Weightage> nifty50, string _defaultDate)
        {
            //test to read data
            BulkEntity datafromXML = new BulkEntity();
            OBJtoXML xmltoObj = new OBJtoXML();
            ConvertiontoDMADayData objNewListObj = new ConvertiontoDMADayData();
            datafromXML.DMAData = new List<Rank>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathDMA, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false)); 
            }
            List<Rank> dataitem1 = datafromXML.DMAData.Where(x => x.SourceName !=null).ToList();
            List<UIDetailedDMA> objNewListData= objNewListObj.ConvertOnjectToDMAData(dataitem1).ToList();
            return FindChangeInLTPPosation(objNewListData, nifty50);
        }

        private List<UIDetailedDMA> FindChangeInLTPPosation(List<UIDetailedDMA> data, List<Weightage> nifty50)
        {
            List<UIDetailedDMA> listOfUIDetailedDMA = new List<UIDetailedDMA>();
          
            var SourceList = data.Select(x => new {x.SourceName }).Distinct().ToList();
            foreach (var item in SourceList)
            {
                List<UIDetailedDMA> itemList = data.Where(x => x.SourceName == item.SourceName.ToString()).ToList();
                if(itemList.Count>1)
                {
                    if(itemList[0].UniqueString != itemList[1].UniqueString)
                    {
                        string dmaString=DMAChangeStringNew(itemList[0], itemList[1]);
                        Array ary1 = itemList[0].UniqueString.Split('-');
                        int i = 0;
                        foreach (var aryitem in ary1)
                        {
                            if (aryitem.ToString() == "LTP")
                            {
                                if(itemList[1].UniqueString.Split('-')[i].ToString() != "LTP")
                                {
                                    itemList[0].ItemBasedChange = dmaString.Replace("FiftyDMA", "50").Replace("ThirtyDMA", "30").Replace("TwentyDMA", "20").Replace("TenDMA", "10").Replace("FiveDMA", "5").Replace("TwoHundredDMA", "200").Replace("HundredDMA", "100").Replace("LTP","");
                                    List<decimal> GetScoreDMAChangedata = GetScoreDMAChangeList(itemList[0].ItemBasedChange, itemList[0].SourceName, nifty50);
                                    itemList[0].ChangeScore = GetScoreDMAChangedata[0];
                                    itemList[0].IndexScore = GetScoreDMAChangedata[1];
                                    listOfUIDetailedDMA.Add(itemList[0]);
                                    break;
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            return listOfUIDetailedDMA;
        }
       
        private List<decimal> GetScoreDMAChangeList(string name, string sourceName,List<Weightage> nifty50)
        {
            List<decimal> obj = new List<decimal>();
            int score = 0;
            foreach (var newitem in name.Split('#').ToArray())
            {
                if (newitem.Split(',').Length > 1)
                {
                    if (newitem.Split(',')[0].Trim() == "+")
                    {
                        if (newitem.Split(',')[1].Trim() == "5" || newitem.Split(',')[1].Trim() == "10" || newitem.Split(',')[1].Trim() == "20")
                        {
                            //nothing to do here
                        }
                        else
                        {
                            if(newitem.Split(',')[1].Trim() =="30")
                            {
                                score = score + 2;
                            }
                            else if (newitem.Split(',')[1].Trim() == "50")
                            {
                                score = score + 3;
                            }
                            else if (newitem.Split(',')[1].Trim() == "100")
                            {
                                score = score + 2;
                            }
                            else if (newitem.Split(',')[1].Trim() == "200")
                            {
                                score = score + 3;
                            }
                        }
                    }
                    else
                    {
                        if (newitem.Split(',')[1].Trim() == "5" || newitem.Split(',')[1].Trim() == "10" || newitem.Split(',')[1].Trim() == "20")
                        {
                            //nothing to do here
                        }
                        else
                        {
                            if (newitem.Split(',')[1].Trim() == "30")
                            {
                                score = score - 2;
                            }
                            else if (newitem.Split(',')[1].Trim() == "50")
                            {
                                score = score - 3;
                            }
                            else if (newitem.Split(',')[1].Trim() == "100")
                            {
                                score = score - 2;
                            }
                            else if (newitem.Split(',')[1].Trim() == "200")
                            {
                                score = score - 3;
                            }
                        }
                    }
                }
            }

            List<Weightage> objData = nifty50.Where(x=>x.Name== sourceName).ToList();
            string sourceValue = string.Empty;
            if(objData == null || objData.Count == 0)
            {
                sourceValue = "0";
            }
            else
            {
                sourceValue = objData[0].Value;
            }
            obj.Add(Convert.ToDecimal(score));
            obj.Add(Convert.ToDecimal(score) * Convert.ToDecimal(sourceValue));
            return obj;
        }

        private string DMAChangeStringNew(UIDetailedDMA today, UIDetailedDMA yesterday)
        {
            string change = string.Empty;
            if (today.FiveDMAColour != yesterday.FiveDMAColour)
            {
                string symbol = string.Empty;              

                if(today.FiveDMAColour == "maroon")
                {
                    symbol = "-," + "FiveDMA";
                }
                else if (today.FiveDMAColour == "royalblue")
                {
                    symbol = "+," + "FiveDMA";
                }
                change = change + "#" + symbol;
            }


            if (today.TenDMAColour != yesterday.TenDMAColour)
            {
                string symbol = string.Empty;
               
                if (today.TenDMAColour == "maroon")
                {
                    symbol = "-," + "TenDMA";
                }
                else if (today.TenDMAColour == "royalblue")
                {
                    symbol = "+," + "TenDMA";
                }
                change = change + "#" + symbol;

            }

            if (today.TwentyDMAColour != yesterday.TwentyDMAColour)
            {
                string symbol = string.Empty;
               

                if (today.TwentyDMAColour == "maroon")
                {
                    symbol = "-," + "TwentyDMA";
                }
                else if (today.TwentyDMAColour == "royalblue")
                {
                    symbol = "+," + "TwentyDMA";
                }
                change = change + "#" + symbol;
            }


            if (today.ThirtyDMAColour != yesterday.ThirtyDMAColour)
            {
                string symbol = string.Empty;
               

                if (today.ThirtyDMAColour == "maroon")
                {
                    symbol = "-," + "ThirtyDMA";
                }
                else if (today.ThirtyDMAColour == "royalblue")
                {
                    symbol = "+," + "ThirtyDMA";
                }
                change = change + "#" + symbol;
            }


            if (today.FiftyDMAColour != yesterday.FiftyDMAColour)
            {
                string symbol = string.Empty;
              

                if (today.FiftyDMAColour == "maroon")
                {
                    symbol = "-," + "FiftyDMA";
                }
                else if (today.FiftyDMAColour == "royalblue")
                {
                    symbol = "+," + "FiftyDMA";
                }
                change = change + "#" + symbol;
            }

            if (today.HundredDMAColour != yesterday.HundredDMAColour)
            {
                string symbol = string.Empty;
                

                if (today.HundredDMAColour == "maroon")
                {
                    symbol = "-," + "HundredDMA";
                }
                else if (today.HundredDMAColour == "royalblue")
                {
                    symbol = "+," + "HundredDMA";
                }

                change = change + "#" + symbol;
            }

            if (today.TwoHundredDMAColour != yesterday.TwoHundredDMAColour)
            {
                string symbol = string.Empty;
               

                if (today.TwoHundredDMAColour == "maroon")
                {
                    symbol = "-," + "TwoHundredDMA";
                }
                else if (today.TwoHundredDMAColour == "royalblue")
                {
                    symbol = "+," + "TwoHundredDMA";
                }

                change = change + "#" + symbol;
            }
            return change;
        }

        private string DMAChangeString(string today, string yesterday,string sourceName)
        {
            
            string createText = today +"@@" + yesterday+ "@@" +sourceName + Environment.NewLine;
            File.AppendAllText(@"D:\datafiletotextdma.txt", createText);

            return "";
        }
    }
}
