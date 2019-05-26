using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using DataContext;
using System.IO;
using Entity;


namespace QuickWatchMethods
{
    public class LogicSaparation
    {
        string _masterDatapathOI = ConfigurationManager.AppSettings["MasterDatapathOI"];
        string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
        string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
       // int _resCount = Convert.ToInt32(ConfigurationManager.AppSettings["ResCount"]);
        string _bankNiftyPath = ConfigurationManager.AppSettings["BankNiftyPath"];
        string _bankNiftyIndexStocks = ConfigurationManager.AppSettings["BankNiftyIndexStocks"];
        string masterMCpath = ConfigurationManager.AppSettings["MasterMCpath"];

        public QueryBinder BankNifty(string _defaultDate)
        {
            //Cache1
            QueryBinder binder = new QueryBinder();
            WeightageClass obj = new WeightageClass();
            List<Weightage> weightageData = new List<Weightage>();
            weightageData = obj.WeightageBNData(_bankNiftyPath);
            BulkData objList = new BulkData();
            binder.PosativeData = objList.GetBasicData(weightageData, _bankNiftyIndexStocks, _masterDatapathBasic,_defaultDate);
            List<OIDetails> objx = new List<OIDetails>();
            List<UIDetailedDMA> objy = new List<UIDetailedDMA>();
            binder.CheckPriceStrongOIStroing = objx;
            binder.UIDetailedDMA = objy;
            return binder;
        }
        public QueryBinder Index(string id, string _defaultDate)
        {
            //Cache2
            QueryBinder binder = new QueryBinder();
            BulkData objList = new BulkData();
            binder.PosativeData = objList.GetBasicData(new List<Weightage>(), id, _masterDatapathBasic, _defaultDate);
            binder.CheckPriceStrongOIStroing = objList.GetOpenInterestData(id, _masterDatapathOI, _defaultDate);
            //binder.CheckLTPHasFirstPlace = objList.GetDMAData(id, _masterDatapathDMA);
            binder.UIDetailedDMA = objList.GetConvertObjectToDMAData(id, _masterDatapathDMA, _defaultDate);
            binder.AvgVolums=GenerateAvgVolumsData(binder.PosativeData, binder.CheckPriceStrongOIStroing);
            return binder;
        }

        public QueryBinder MCData(string _defaultDate)
        {
            //Cache2
            QueryBinder binder = new QueryBinder();
            BulkData objList = new BulkData();
            binder.MCData = objList.MCData(masterMCpath, _defaultDate);
            return binder;
        }

        public QueryBinder AvgAll(string _defaultDate)
        {
            //Cache2
            QueryBinder binder = new QueryBinder();

            BulkData objList = new BulkData();
            binder.PosativeData = objList.GetBasicAvgAll(new List<Weightage>(),_masterDatapathBasic, _defaultDate).Where(x=>x.Code !=null).ToList();
            binder.CheckPriceStrongOIStroing = objList.GetOIAvgAll(_masterDatapathOI, _defaultDate).Where(x => x.CompanyCode != null).ToList();
            binder.AvgVolums = GenerateAvgVolumsDataForAll(binder.PosativeData, binder.CheckPriceStrongOIStroing).Where(x=>x.IsBasicBuy ==true || x.IsOIBuy == true).ToList();

            return binder;
        }

        private QueryBinder GenerateDynamicList(QueryBinder binder, QueryBinder binderList)
        {
            QueryBinder obj = new QueryBinder();



            return obj;
        }

        private List<AvgVolums> GenerateAvgVolumsDataForAll(List<CompanyDetails> BasicData, List<OIDetails> Oidata)
        {
            List<AvgVolums> masterList = new List<AvgVolums>();
            List<AvgVolums> templist = new List<AvgVolums>();

            foreach (var item in BasicData.GroupBy(g => new { g.Code }).Select(g => g.First()).ToList())
            {
                List<CompanyDetails> basicData = BasicData.Where(x => x.Code == item.Code).ToList();
                List<OIDetails> oidata = Oidata.Where(x => x.CompanyCode == item.Code).ToList();

                templist = GenerateAvgVolumsData(basicData, oidata);
                templist = IsBasicBuy(templist);
                masterList.AddRange(templist);
            }



            //Regenerating list to add buy check for both basic and Io values



            return masterList;
        }


        private List<AvgVolums> IsBasicBuy(List<AvgVolums> avgVolums)
        {
            bool basicvalue = true;
            bool Iovalue = true;


            for (int i = 0; i < avgVolums.Count-1; i++)
            {
                if(basicvalue)
                {
                    if (Convert.ToDecimal(avgVolums[0].TotalVolume) < Convert.ToDecimal(avgVolums[i + 1].TotalVolume))
                    {
                        basicvalue = false;
                    }
                }

                if (Iovalue)
                {
                    if (Convert.ToDecimal(avgVolums[0].OITotalVolume) < Convert.ToDecimal(avgVolums[i + 1].OITotalVolume))
                    {
                        Iovalue = false;
                    }
                }
            }
            avgVolums.ToList().ForEach(c => c.IsBasicBuy = basicvalue);
            avgVolums.ToList().ForEach(c => c.IsOIBuy = Iovalue);

            return avgVolums;
        }

        private bool IsOIBuy(List<OIDetails> Oidata)
        {
            bool value = false;
            for (int i = 0; i < Oidata.Count - 1; i++)
            {
                if (Convert.ToDecimal(Oidata[0].NumberOfContractsTraded) >= Convert.ToDecimal(Oidata[i + 1].NumberOfContractsTraded))
                {
                    value = true;
                }
                else
                {
                    value = false;
                }
            }
            return value;
        }

        private List<AvgVolums> GenerateAvgVolumsData(List<CompanyDetails> BasicData, List<OIDetails> Oidata)
        {
            List<AvgVolums> objList = new List<AvgVolums>();
            List<DateTime> dates = new List<DateTime>();
            AvgVolums obj = null;
           

            foreach (var item in BasicData)
            {
               if(!dates.Contains(item.BasicSavedTimeStamp.Date))
                {
                    dates.Add(item.BasicSavedTimeStamp.Date);
                    var smallData = Oidata.Where(x => x.CompanyCode == item.Code && x.OISavedTimeStamp.Date == item.BasicSavedTimeStamp.Date).ToList();

                    if (smallData.Count > 0)
                    {
                        obj = new AvgVolums();
                        obj.Code = item.Code;
                        obj.LTPDate = item.BasicSavedTimeStamp;
                       if (item.TotalVolume.Contains('}'))
                        {
                            obj.TotalVolume = item.TotalVolume.Split('}')[0].Remove(item.TotalVolume.Split('}')[0].Length - 1);
                        }
                        else
                        {
                            obj.TotalVolume = item.TotalVolume;
                        }
                      
                        obj.BasicPercentage = item.pChange;

                        obj.OITotalVolume = smallData[0].NumberOfContractsTraded;
                        obj.IOPercentage = smallData[0].PChange;


                        objList.Add(obj);
                    }
                }
            }
            return objList;
        }



        public QueryBinder Discounting(string id, string _defaultDate)
        {
            //Cache2
            QueryBinder binder = new QueryBinder();
            BulkData objList = new BulkData();
            binder.PosativeData = objList.GetBasicData(new List<Weightage>(), id, _masterDatapathBasic, _defaultDate);
            binder.CheckPriceStrongOIStroing = objList.GetOpenInterestData(id, _masterDatapathOI, _defaultDate);
            return binder;
        }

        public QueryBinder FastMovers(string _defaultDate)
        {
            //Cache3
            QueryBinder binder = new QueryBinder();
            BulkData objList = new BulkData();
            binder.PosativeData = objList.GetBasicData(new List<Weightage>(), "All", _masterDatapathBasic, _defaultDate);
            return binder;
        }

        public QueryBinder TopVolumes(string _defaultDate)
        {  
            //Cache4
            QueryBinder binder = new QueryBinder();
            BulkData objList = new BulkData();
            List<CompanyDetails> data = objList.GetBasicData(new List<Weightage>(), "All", _masterDatapathBasic, _defaultDate).Where(x => x.BasicSavedTimeStamp.Date == DateTime.Now.Date).ToList();
            int qId = data.Max(x => x.Id);
            binder.PosativeData = data.Where(x => x.Id == qId).OrderByDescending(x => Convert.ToDecimal(x.CurrentPrevdayVolumePercentage)).Take(20).ToList();
            // binder.NegativeData = data.Where(x => x.Id == qId).OrderBy(x => Convert.ToDecimal(x.CurrentPrevdayVolumePercentage)).Take(5).ToList();

            return binder;
        }
        public QueryBinder TodayPicker(string _defaultDate)
        {   
            //Cache5
            QueryBinder binder = new QueryBinder();
            RBOData objData = new RBOData();
            List<RBOEntity> data = objData.RBO(_defaultDate);
            List<CompanyDetails> todayPickerData = new List<CompanyDetails>();
            BulkData objList = new BulkData();
            List<CompanyDetails> data1 = objList.GetBasicData(new List<Weightage>(), "All", _masterDatapathBasic, _defaultDate).Where(x => x.BasicSavedTimeStamp.Date == DateTime.Now.Date).ToList();
            int qId = data1.Max(x => x.Id);
            List<CompanyDetails> newDataList = data1.Where(x => x.Id == qId).OrderByDescending(x => Convert.ToDecimal(x.CurrentPrevdayVolumePercentage)).Take(30).ToList();
            foreach (var newItem in newDataList)
            {
                foreach (var item in data.Where(x => Convert.ToDecimal(x.BOPercentage) > 95).OrderByDescending(x => Convert.ToDecimal(x.BOPercentage)))
                {
                    if (item.Code == newItem.Code)
                    {
                        todayPickerData.Add(newItem); break;
                    }
                }
            }
            binder.PosativeData = todayPickerData.ToList();
            return binder;
        }
        public QueryBinder RBO(string _defaultDate)
        {
            //Cache6
            QueryBinder binder = new QueryBinder();
            RBOData objData = new RBOData();
            List<RBOEntity> data = objData.RBO(_defaultDate);
            binder.RBOData = data.Where(x => Convert.ToDecimal(x.BOPercentage) > 95).OrderByDescending(x => Convert.ToDecimal(x.BOPercentage)).ToList();
            return binder;
        }

        public QueryBinder FullDMA(List<Weightage> nifty50, List<Weightage> bankNifty, string _defaultDate)
        {
            //Cache7
            QueryBinder binder = new QueryBinder();
            BasicDataLogic objList = new BasicDataLogic();
            OILogic objOIlist = new OILogic();
            DMALogic objDmaLogic = new DMALogic();

            binder.CheckLTPHasFirstPlace = objDmaLogic.CheckLTPHasFirstPlace(_masterDatapathDMA, _defaultDate).ToList();
            binder.CheckLTPHasSecondPlace = objDmaLogic.CheckLTPHasSecondPlace(_masterDatapathDMA, _defaultDate).ToList();
            binder.CheckLTPHasLastPlace = objDmaLogic.CheckLTPHasLastPlace(_masterDatapathDMA, _defaultDate).ToList();
            binder.CheckLTPHasBeforeLastPlace = objDmaLogic.CheckLTPHasBeforeLastPlace(_masterDatapathDMA, _defaultDate).ToList();
            binder.UIDetailedDMA = objDmaLogic.ChangeDMAPosation(_masterDatapathDMA, nifty50, _defaultDate).OrderByDescending(x=>x.ChangeScore).ToList();

            binder.NiftyDMAData = objDmaLogic.BankniftyDMAScore(_masterDatapathDMA, nifty50, _defaultDate).OrderByDescending(x=>x.DMAScore).ToList();
            binder.BankNiftyDMAData = objDmaLogic.BankniftyDMAScore(_masterDatapathDMA, bankNifty, _defaultDate).OrderByDescending(x => x.DMAScore).ToList();

            return binder;
        }
        public QueryBinder DynamicList(string _defaultDate)
        {   
            //Cache7
            QueryBinder binder = new QueryBinder();
            BasicDataLogic objList = new BasicDataLogic();
            OILogic objOIlist = new OILogic();
            DMALogic objDmaLogic = new DMALogic();

            binder.PosativeData = objList.CheckBuySignals(_masterDatapathBasic, _defaultDate).OrderByDescending(x =>Convert.ToDecimal(x.pChange)).ToList();
            binder.NegativeData = objList.CheckSellSignals(_masterDatapathBasic, _defaultDate).OrderBy(x => Convert.ToDecimal(x.pChange)).ToList();
            binder.HighBuying = objList.CheckHighBuying(_masterDatapathBasic, _defaultDate).ToList();
            binder.HighSelling = objList.CheckHighSelling(_masterDatapathBasic, _defaultDate).ToList();

            binder.CheckPriceStrongOIStroing = objOIlist.CheckPriceStrongOIStroing(_masterDatapathOI, _defaultDate).OrderByDescending(x=> Convert.ToDecimal(x.PchangeinOpenInterest)).ToList();
            binder.CheckPriceWeekOIStroing = objOIlist.CheckPriceWeekOIStroing(_masterDatapathOI, _defaultDate).OrderByDescending(x => Convert.ToDecimal(x.PchangeinOpenInterest)).ToList();
            binder.CheckPriceStrongOIWeek = objOIlist.CheckPriceStrongOIWeek(_masterDatapathOI, _defaultDate).OrderBy(x => Convert.ToDecimal(x.PchangeinOpenInterest)).ToList();
            binder.CheckPriceWeekIOWeek = objOIlist.CheckPriceWeekIOWeek(_masterDatapathOI, _defaultDate).OrderBy(x => Convert.ToDecimal(x.PchangeinOpenInterest)).ToList();
            return binder;
        }
        public List<string> DateCollection()
        {
            string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
            string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];
            string[] _basic = Directory.GetFiles(_masterDatapathBasic, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            string[] _Dma = Directory.GetFiles(_masterDatapathDMA, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            List<string> datesbasic = new List<string>();
            List<string> datesDma = new List<string>();

            foreach (var item in _basic)
            {
                string[] values = item.Replace("Q-Dashboard", "QDashboard").Split('-');
                string date = values[1] + "-" + values[2] + "-" + values[3];
                if (!datesbasic.Contains(date))
                {
                    datesbasic.Add(date);
                }
            }
            foreach (var item2 in _Dma)
            {
                string[] values = item2.Replace("Q-Dashboard", "QDashboard").Split('-');
                string date = values[1] + "-" + values[2] + "-" + values[3];
                if (!datesDma.Contains(date))
                {
                    datesDma.Add(date);
                }
            }

            if (datesbasic.Count != datesDma.Count)
            {
                throw new InvalidCastException();
            }
            return datesbasic;
        }

    }
}  

