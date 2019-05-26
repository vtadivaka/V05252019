using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
using System.IO;
using ObjToXml;
using DataContext;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace QuickApi.Controllers
{
    public class OptionChainController : ApiController
    {

        private async void AzRedisCacheExample()
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                "azquickwatch.redis.cache.windows.net:6380,password=NAFrdrom50XCu+vSAwPfQRjiwQ560umWW+j39UN89YU=,ssl=True,abortConnect=False");
            IDatabase cache = connection.GetDatabase();
            var key1 = await cache.StringGetAsync("optionsData");



            // Perform cache operations using the cache object...
            // Simple put of integral data types into the cache
            //cache.StringSet("key1", "dsyavdadsahda");
           // cache.StringSet("key1", "dsyavdadsahda", TimeSpan.FromHours(1));
           // cache.StringSet("key2", 25);

           // var exists = cache.KeyExists("key1");

            // Simple get of data types from the cache
           // var key1 =await cache.StringGetAsync("key1");
           // int key2 = (int)cache.StringGet("key2");
        }
        [HttpGet]
        public List<OptionChain2> GetCurrentN()
        {
            
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
               "azquickwatch.redis.cache.windows.net:6380,password=NAFrdrom50XCu+vSAwPfQRjiwQ560umWW+j39UN89YU=,ssl=True,abortConnect=False,syncTimeout=600000,connectRetry=3,connecttimeout = 600000");
           
            IDatabase cache = connection.GetDatabase();
         

            List<OptionChain> optionsData = JsonConvert.DeserializeObject<List<OptionChain>>(cache.StringGet("optionsData"));
            optionsData = optionsData.Where(x => x.SourceName == "NIFTY").ToList();
            optionsData = optionsData.OrderBy(x => Convert.ToDateTime(x.TimeStamp)).ToList();
            List<OptionChain2> optionsData2 = GetPersonalisedoptionsData(optionsData);
            return optionsData2.OrderByDescending(x => Convert.ToDateTime(x.TimeStamp)).ToList();
        }

        private List<OptionChain2> GetDateWiseData(List<OptionChain2> optionsData)
        {
            List<OptionChain2> ObjOptionChain = new List<OptionChain2>();
            string date = string.Empty;

            foreach (var item in optionsData)
            {
                if (item.AsOnTime != null && item.AsOnTime != "" && date != item.AsOnTime.Remove(item.AsOnTime.Length - 9).Trim())
                {
                    date = item.AsOnTime.Remove(item.AsOnTime.Length - 9).Trim();
                    ObjOptionChain.Add(item);
                }
            }
            return ObjOptionChain;
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
                        objChain.Strikes = optionsData[i].Strikes;
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
    }
}