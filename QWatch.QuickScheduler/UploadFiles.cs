using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.File;


namespace AZQuickScheduler
{
    public class UploadFiles
    {
        //static string accountname = ConfigurationManager.AppSettings["accountName"];
        //static string key = ConfigurationManager.AppSettings["key"];

        //public static CloudStorageAccount GetConnectionString()
        //{
        //    string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountname, key);
        //    return CloudStorageAccount.Parse(connectionString);
        //}
        //public void UploadFiletoFileShare(string fileShareName)
        //{
        //    //Connect to Azure
        //    CloudStorageAccount storageAccount = GetConnectionString();

        //    // Create a CloudFileClient object for credentialed access to Azure Files.
        //    CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

        //    // Get a reference to the file share we created previously.
        //    CloudFileShare share = fileClient.GetShareReference(fileShareName);

        //    // Ensure that the share exists.
        //    if (share.Exists())
        //    {
        //        // Get a reference to the root directory for the share.
        //        CloudFileDirectory rootDir = share.GetRootDirectoryReference();

        //        // Get a reference to the directory we created previously.
        //        CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("CustomLogs");

        //        // Ensure that the directory exists.
        //        if (sampleDir.Exists())
        //        {
        //            // Get a reference to the file we created previously.
        //            CloudFile file = sampleDir.GetFileReference("Log1.txt");

        //            // Ensure that the file exists.
        //            if (file.Exists())
        //            {
        //                // Write the contents of the file to the console window.
        //                Console.WriteLine(file.UploadFromFileAsync());
        //            }
        //        }
        //    }

        //}
    }
}
