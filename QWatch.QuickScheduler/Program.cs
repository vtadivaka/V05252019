using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Entity;
using System.IO;
using ObjToXml;
using DataContext;
using System.Configuration;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace AZQuickScheduler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Program objProgram = new Program();
            objProgram.AzRedisCacheGenerator();


            Console.WriteLine("Process Started.");
            var config = new JobHostConfiguration();
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            LoadData();
            host.RunAndBlock();
        }
        public static void LoadData()
        {
            OptionChainClass obj = new OptionChainClass();
            string path = ConfigurationManager.AppSettings["Dates"];
            string[] dates = path.Split(',');
            string masterFilePath = ConfigurationManager.AppSettings["MasterFilePath"];
            bool isLoadAllData = false;

            string bankNiftyStaticData = ConfigurationManager.AppSettings["BankNiftyStaticData"]; ;
            string niftyStaticData = ConfigurationManager.AppSettings["NiftyStaticData"]; ;
            string liveMarketUrl = ConfigurationManager.AppSettings["LiveMarketUrl"]; ;

            Console.WriteLine(path);
            Console.WriteLine(masterFilePath);
            Console.WriteLine(bankNiftyStaticData);
            Console.WriteLine(niftyStaticData);
            Console.WriteLine(liveMarketUrl);

            try
            {
                List<OptionChain> chainData = obj.LoadFullOptionChainData(masterFilePath, dates, isLoadAllData, bankNiftyStaticData, niftyStaticData, liveMarketUrl);
               
                OBJtoXML xmltoObj = new OBJtoXML();
                xmltoObj.CreateOptionsXML(chainData, masterFilePath, "LoadFullOptionChainData", true);
                //Code to load file to Azure blob storage account
                Console.WriteLine("Loaded data sucussfully.");
                Console.WriteLine("Thread locked.");
                System.Threading.Thread.Sleep(180000);
                Console.WriteLine("Thread lock released.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Process completed");
            }
        }
        private void AzRedisCacheGenerator()
        {
            int workerThreads = 500;
            int iocpThreads = 500;
            System.Threading.ThreadPool.SetMinThreads(workerThreads, iocpThreads);

            string masterFilePath = ConfigurationManager.AppSettings["MasterFilePath"];
            string azMasterFilePath = ConfigurationManager.AppSettings["AzMasterFilePath"];

            bool isAzureCall = true; //need to call this one from the config
            List<OptionChain> optionsData = new List<OptionChain>();
            optionsData = LoadOptionsFullData(azMasterFilePath, isAzureCall);

            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(
                "azquickwatch.redis.cache.windows.net:6380,password=NAFrdrom50XCu+vSAwPfQRjiwQ560umWW+j39UN89YU=,ssl=True,abortConnect=False");
            IDatabase cache = connection.GetDatabase();
            var serializedObject = JsonConvert.SerializeObject(optionsData);

            // Perform cache operations using the cache object...
            // Simple put of integral data types into the cache
            cache.StringSet("optionsData2", serializedObject);
        }
        private List<DateValuePair> ReadAllFile(string masterFilePath)
        {
            DateValuePair textItem = new DateValuePair();
            List<DateValuePair> txtFiles = new List<DateValuePair>();
            // Retrieve storage account information from connection string
            CloudStorageAccount storageAccount = BlobStorage.Common.CreateStorageAccountFromConnectionString();

            // Create a blob client for interacting with the blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Create a container for organizing blobs within the storage account.
            CloudBlobContainer container = blobClient.GetContainerReference("oidata");
            foreach (var item in container.ListBlobs())
            {
                textItem = new DateValuePair();
                textItem.Value = item.Uri.ToString();
                textItem.LastModifiedDate = ((CloudBlob)item).Properties.LastModified.Value.UtcDateTime;
                txtFiles.Add(textItem);
            }
            return txtFiles.OrderByDescending(d => d.LastModifiedDate).Take(50).ToList();
        }

        private List<OptionChain> LoadOptionsFullData(string masterFilePath, bool isAzure)
        {
            List<OptionChain> optionsData = new List<OptionChain>();
            OBJtoXML xmltoObj = new OBJtoXML();
            if (isAzure)
            {
                List<DateValuePair> txtFiles = ReadAllFile(masterFilePath);
                foreach (DateValuePair currentFile in txtFiles)
                {
                    optionsData.AddRange((List<OptionChain>)xmltoObj.CreateOptionsObject(optionsData, currentFile.Value, true));
                }
            }
            else
            {
                string[] txtFiles = Directory.GetFiles(masterFilePath, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

                //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
                foreach (string currentFile in txtFiles)
                {
                    optionsData.AddRange((List<OptionChain>)xmltoObj.CreateOptionsObject(optionsData, currentFile, true));
                }
            }
            return optionsData;
        }
    }
}
