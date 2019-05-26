using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Entity;

namespace DataContext
{
    public class Dma
    {
        public void ReplaceMasterFileNames()
        {
            //Need to full data
            string[] filesData = System.IO.Directory.GetFiles(@"D:\Analyzer\MasterData20022018\", "*.csv", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var item in filesData)
            {
                string name = item.Split('-')[item.Split('-').Length - 1];

                if (name == "AUTOALLN.csv")
                {
                    name = "2017BAJAJ-AUTOALLN.csv";
                }

                if (name == "NALLN.csv")
                {
                    name = "2017MCDOWELL - NALLN.csv";
                }

                
                string newname = name.Substring(4, name.Length - 12).Trim();
                string newpath = @"D:\Analyzer\MasterData20022018\" + newname + ".csv";
                System.IO.File.Move(item, newpath);
            }
        }


        public List<Rank> RefreshDmaData(string fiftyFilesPath)
        {
            List<DmaData> objDmaData = new List<DmaData>();
            for (int day = 31; day >1; day--)
            {
                //Need to full data
                string[] filesData = System.IO.Directory.GetFiles(fiftyFilesPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);
                foreach (var item in filesData)
                {
                    //string ltpPrice = objLtpPrices.Where(x => x.KeyName == item.Split('\\')[item.Split('\\').Length - 1].Split('.')[0]).ToList()[0].Value;
                    objDmaData.Add(GetAllFile(item.ToString(), "MASTERDATAREFRESH","",day));
                    
                }
            }
            //Need to load daily based data
           return GetRanks(objDmaData); 
        }


        public List<Rank> GetDmaData(string fiftyFilesPath, string LtpfilePath, string dailyfullFilesPath)
        {
            List<DmaData> objDmaData = new List<DmaData>();

            //Need to current price
            List<KeyValue> objLtpPrices = GetLtpfileData(LtpfilePath); // @"C:\Users\TadivakaV\Downloads\data.csv"

          
            //Need to full data
            string[] filesData = System.IO.Directory.GetFiles(fiftyFilesPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var item in filesData)
            {
                string name = item.Split('\\')[item.Split('\\').Length - 1].Split('.')[0];
                string ltpPrice = objLtpPrices.Where(x => x.KeyName == name).ToList()[0].Value;
                objDmaData.Add(GetAllFile(item.ToString(), ltpPrice,"",999));
            }
            //Need to load daily based data
            return GetRanks(objDmaData);
        }
        public List<Rank> GetDmaData(string fiftyFilesPath, List<KeyValue> objLtpPrices, string dailyfullFilesPath)
        {
            List<DmaData> objDmaData = new List<DmaData>();
            //Need to full data
            string[] filesData = System.IO.Directory.GetFiles(fiftyFilesPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);

            foreach (var item in objLtpPrices)
            {
                try
                {
                    string name = item.KeyName;
                    string ltpPrice = item.Value;
                    string closedPrice = item.ClosedPrice;
                    string fileName = "\\" + name + ".csv";
                    var filePath = filesData.Where(x => x.Contains(fileName)).ToList();
                    if(filePath.Count==1)
                    {
                        objDmaData.Add(GetAllFile(filePath[0].ToString(), ltpPrice, closedPrice, 999));
                    }
                }
                catch(Exception ex)
                {

                }
            }
            //Need to load daily based data
            return GetRanks(objDmaData);
        }


        private List<Rank>  GetRanks(List<DmaData>  data)
        {
            List<Rank> objRanks = new List<Rank>();
            Rank objRank = null;
            Dictionary<string, decimal> objPair = null;
            int UniqueNumber = 1;
            foreach (var item in data)
            {
                objPair = new Dictionary<string, decimal>();
                objPair.Add("LTP", Convert.ToDecimal(item.LTP));
                objPair.Add("FiveDMA", Convert.ToDecimal(item.FiveDMA));
                objPair.Add("TenDMA", Convert.ToDecimal(item.TenDMA));
                objPair.Add("TwentyDMA", Convert.ToDecimal(item.TwentyDMA));
                objPair.Add("ThirtyDMA", Convert.ToDecimal(item.ThirtyDMA));
                objPair.Add("FiftyDMA", Convert.ToDecimal(item.FiftyDMA));
                objPair.Add("HundredDMA", Convert.ToDecimal(item.HundredDMA));
                objPair.Add("TwoHundredDMA", Convert.ToDecimal(item.TwoHundredDMA));
                var ordered = objPair.OrderByDescending(x => x.Value);

                StringBuilder sb = new StringBuilder();
                foreach (var itm in ordered)
                {
                    sb.Append(itm.Key);
                    sb.Append("-");
                }
               
                int RankId = 1;
                bool isLtpDone = false;
                foreach (var rank in ordered)
                {
                    objRank = new Rank();
                    objRank.RankSavedTimeStamp = DateTime.Now;
                    
                    objRank.Day = rank.Key;
                    objRank.Value = RoundMethod(rank.Value.ToString());
                    objRank.RankId = RankId;
                    objRank.UniqueNumber = UniqueNumber;
                    objRank.SourceName = item.SourceName;
                    objRank.LTPDate = item.LTPDate;
                    objRank.UniqueString = sb.ToString();
                    if (rank.Key == "LTP" && isLtpDone == false)
                    {
                        objRank.Colour = "yellow"; isLtpDone = true;
                    }
                    else if (isLtpDone == true && rank.Key != "LTP")
                    {
                        objRank.Colour = "royalblue";
                    }
                    else
                    {
                        objRank.Colour = "maroon";
                    }
                    objRank.Closed = item.ClosedPrice;
                    objRanks.Add(objRank);
                    RankId++; UniqueNumber++;
                }
            }

            return objRanks;
        }


        public string RoundMethod(string text)
        {
            decimal value;
            if (decimal.TryParse(text, out value))
            {
                value = Math.Round(value,2);
                text = value.ToString();
            }
            return text;
        }
        private List<KeyValue> GetLtpfileData(string LtpfilePath)
        {
            List<KeyValue> objList = new List<KeyValue>();
            using (var reader = new StreamReader(LtpfilePath))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                     var line = reader.ReadLine().Replace(",\"", "\"!");
                     var values = line.Split('!');
                  
                    KeyValue newObj = null;
                    if (count > 0)
                    {
                        newObj = new KeyValue();
                        newObj.KeyName =values[0].Replace("\"", "").Trim();
                        newObj.Value = values[4].Replace("\"", "").Trim();
                        objList.Add(newObj);
                    }
                    count++;
                }
            }
            return objList;
        }
        private DmaData GetAllFile(string filePath, string ltp,string closedPrice, int day)
        {

            List<KeyData> objList = new List<KeyData>();
            DmaData objDMA = new DmaData();

            string[] allLines = File.ReadAllLines(filePath);

            //string[] allLines = File.ReadAllLines(@"C:\Users\TadivakaV\Downloads\01-07-2016-TO-30-06-2017DRREDDYALLN.csv");

            var query = from line in allLines
                        let data = line.Split(',')
                        select new
                        {
                            Symbol = data[0],
                            Series = data[1],
                            Date   = data[2],
                            PrevClose = data[3],
                            OpenPrice = data[4],
                            Highrice = data[5],
                            LowPrice = data[6],
                            LastPrice = data[7],
                            ClosePrice = data[8],
                            AveragePrice = data[9],
                            TotalTradedQuantity = data[10],
                            Turnover = data[11],
                            NoOfTrades = data[12],
                            DeliverableQty = data[13],
                            DlyQttoTradedQty = data[14]
                        };
            KeyData newObj = null;
            int count=0;
            try
            {
                foreach (var s in query)
                {
                    try
                    {
                        if (count > 0 && s.Series.ToString().Replace("\"", "").Trim() == "EQ")
                        {

                            
                                newObj = new KeyData();
                                newObj.SourceName = s.Symbol.ToString().Replace("\"", "").Trim();
                                string iDate = s.Date.ToString().Replace("\"", "").Trim();
                                DateTime oDate = DateTime.Parse(iDate);
                                newObj.Date = oDate;
                                newObj.ClosedPrice = s.ClosePrice.ToString().Replace("\"", "").Trim();
                                objList.Add(newObj);
                            
                        }
                        count++;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            objList =objList.OrderByDescending(o => o.Date).ToList();

            if (ltp=="MASTERDATAREFRESH")
            {
                ltp = objList[day-1].ClosedPrice;
                objDMA.LTPDate = objList[day - 1].Date;
                objList.RemoveRange(0, day);
            }
            else
            {
                DateTime time1 = DateTime.Now;
                string currentDate = time1.Day + "-" + time1.Month + "-" + time1.Year;
                string searchDate1 = time1.ToString().Replace("\"", "").Trim();
                DateTime oDate = DateTime.Parse(searchDate1);
                objDMA.LTPDate = oDate;
            }

            KeyData keyData = new KeyData();
            keyData.ClosedPrice= (closedPrice == "0.00") ? ltp : closedPrice;
            objList.Insert(0, keyData);

            objDMA.FiveDMA = CalcAvg(objList, 5).ToString();
            objDMA.TenDMA = CalcAvg(objList, 10).ToString();
            objDMA.TwentyDMA = CalcAvg(objList, 20).ToString();
            objDMA.ThirtyDMA = CalcAvg(objList, 30).ToString();
            objDMA.FiftyDMA = CalcAvg(objList, 50).ToString();
            objDMA.HundredDMA = CalcAvg(objList, 150).ToString(); //HundredDMA actually its 150 day moving average
            objDMA.TwoHundredDMA = CalcAvg(objList, 200).ToString();
            objDMA.SourceName = objList[1].SourceName;
            objDMA.LTP = ltp;
            objDMA.ClosedPrice = closedPrice;
            objDMA.FileName = filePath;
            return objDMA;
        }

        private decimal CalcAvg(List<KeyData> data, int Days)
        {
            decimal sum = 0;
            var dynamicData = data.Take(Days).ToList();
            foreach (var item in dynamicData)
            {
                sum = sum + Convert.ToDecimal(item.ClosedPrice);
            }
            return sum / Days;
        }
    }
}
