using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
using System.IO;
using ObjToXml;
using OptionChainData;
using System.Configuration;
using System.Web.Http;
using System.Runtime.Caching;

namespace AngTest1.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        public IEnumerable<OptionChain2> GetNextBN()
        {
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');
            var cache = MemoryCache.Default;
            List<OptionChain> optionsData = new List<OptionChain>();
            if (cache.Get("dataCache") == null)
            {
                var cachePolicty = new CacheItemPolicy();
                cachePolicty.AbsoluteExpiration = DateTime.Now.AddSeconds(600);
                optionsData = LoadOptionsFullData();
                cache.Add("dataCache", optionsData.ToList(), cachePolicty);
            }

            optionsData = (List<OptionChain>)cache.Get("dataCache");
            optionsData = optionsData.Where(x => x.SourceName == "BANKNIFTY" && x.ExpiryDate == dates[1]).ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2 = optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();

            return optionsData2;
        }

     
       
        public IEnumerable<OptionChain2> GetFinalBN()
        {
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');
            var cache = MemoryCache.Default;
            List<OptionChain> optionsData = new List<OptionChain>();
            if (cache.Get("dataCache") == null)
            {
                var cachePolicty = new CacheItemPolicy();
                cachePolicty.AbsoluteExpiration = DateTime.Now.AddSeconds(600);
                optionsData = LoadOptionsFullData();
                cache.Add("dataCache", optionsData.ToList(), cachePolicty);
            }

            optionsData = (List<OptionChain>)cache.Get("dataCache");
            optionsData = optionsData.Where(x => x.SourceName == "BANKNIFTY" && x.ExpiryDate == dates[0]).ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2 = optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();

            return optionsData2;
        }

      
        public IEnumerable<OptionChain2> GetNextN()
        {
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');
            var cache = MemoryCache.Default;
            List<OptionChain> optionsData = new List<OptionChain>();
            if (cache.Get("dataCache") == null)
            {
                var cachePolicty = new CacheItemPolicy();
                cachePolicty.AbsoluteExpiration = DateTime.Now.AddSeconds(600);
                optionsData = LoadOptionsFullData();
                cache.Add("dataCache", optionsData.ToList(), cachePolicty);
            }

            optionsData = (List<OptionChain>)cache.Get("dataCache");
            optionsData = optionsData.Where(x => x.SourceName == "NIFTY" && x.ExpiryDate == dates[0]).ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            optionsData2 = optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();

            return optionsData2;
        }

        public int GetRefreshData()
        {
            OptionChainClass obj = new OptionChainClass();
            string path = ConfigurationManager.AppSettings["dates"];
            string[] dates = path.Split(',');
            obj.LoadFullOptionChainData(@"D:\Analyzer\MasterData\FullOptionsData\", dates, false);
            var cache = MemoryCache.Default;
            List<OptionChain> optionsData = new List<OptionChain>();
            var cachePolicty = new CacheItemPolicy();
            cachePolicty.AbsoluteExpiration = DateTime.Now.AddSeconds(600);
            optionsData = LoadOptionsFullData();
            cache.Add("dataCache", optionsData.ToList(), cachePolicty);
            return 0;
        }
            private List<OptionChain> LoadOptionsFullData()
        {
            OBJtoXML xmltoObj = new OBJtoXML();
            string[] txtFiles = Directory.GetFiles(@"D:\Analyzer\MasterData\FullOptionsData\", "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<OptionChain> optionsData = new List<OptionChain>();
            foreach (string currentFile in txtFiles)
            {
                optionsData.AddRange((List<OptionChain>)xmltoObj.CreateOptionsObject(optionsData, currentFile));
            }
            return optionsData;
        }

        private List<OptionChain2> GetPersonalisedoptionsData(List<OptionChain> optionsData)
        {
            List<OptionChain2> ObjOptionChain = new List<OptionChain2>();
            OptionChain2 objChain = null;

            string tempAsonDate = string.Empty;

            if (optionsData.Count > 1)
            {
                for (int i = 1; i < optionsData.Count - 1; i++)
                {
                    objChain = new OptionChain2();


                    if (tempAsonDate != optionsData[i].AsOnTime || tempAsonDate == null)
                    {
                        objChain.ExpiryDate = optionsData[i].ExpiryDate;
                        objChain.Percentage = optionsData[i].Percentage;
                        objChain.SourceName = optionsData[i].SourceName;
                        objChain.SourceType = optionsData[i].SourceType;
                        objChain.SourceValue = optionsData[i].SourceValue;
                        //objChain.Strikes = optionsData[i].Strikes;
                        objChain.TimeStamp = optionsData[i].TimeStamp;
                        objChain.UnderlyingValue = optionsData[i].UnderlyingValue;
                        objChain.AsOnTime = optionsData[i].AsOnTime;
                        tempAsonDate = objChain.AsOnTime;

                        objChain.total_cal_ChnginOI = optionsData[i].total_cal_ChnginOI + "(" + (optionsData[i].total_cal_ChnginOI - optionsData[i - 1].total_cal_ChnginOI) + ")";
                        objChain.total_cal_OI = optionsData[i].total_cal_OI + "(" + (optionsData[i].total_cal_OI - optionsData[i - 1].total_cal_OI) + ")";
                        objChain.total_cal_Volume = optionsData[i].total_cal_Volume + "(" + (optionsData[i].total_cal_Volume - optionsData[i - 1].total_cal_Volume) + ")";
                        objChain.total_put_ChnginOI = optionsData[i].total_put_ChnginOI + "(" + (optionsData[i].total_put_ChnginOI - optionsData[i - 1].total_put_ChnginOI) + ")";
                        objChain.total_put_OI = optionsData[i].total_put_OI + "(" + (optionsData[i].total_put_OI - optionsData[i - 1].total_put_OI) + ")";
                        objChain.total_put_Volume = optionsData[i].total_put_Volume + "(" + (optionsData[i].total_put_Volume - optionsData[i - 1].total_put_Volume) + ")";
                        ObjOptionChain.Add(objChain);
                    }
                }
            }
            return ObjOptionChain;
        }
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
