using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;

namespace ObjToXml
{
    public class OBJtoXML
    {
        public void CreateXML(Object YourClassObject, string filePath, string dataType, bool IsAzure)
        {
            if (IsAzure == true)
            {
                // Retrieve storage account information from connection string
                CloudStorageAccount storageAccount = BlobStorage.Common.CreateStorageAccountFromConnectionString();

                // Create a blob client for interacting with the blob service.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Create a container for organizing blobs within the storage account.
                CloudBlobContainer container = blobClient.GetContainerReference(dataType);
                container.CreateIfNotExists();

                DateTime dt = DateTime.Now;
                string fileName = dataType + "-" + dt.Day + "-" + dt.Month + "-" + dt.Year + "-";
                if (dataType == "DMAData")
                {
                    foreach (string file in Directory.GetFiles(filePath))
                    {
                        if (file.Contains(fileName))
                        {
                            File.Delete(file);
                        }
                    }
                }
                int fileCount = container.ListBlobs().Count();// Directory.GetFiles(filePath, "*.txt", SearchOption.AllDirectories).Length; // Will Retrieve count of files XML extension in directry and sub directries
                fileCount = fileCount + 1;
                fileName = fileName + fileCount.ToString() + ".txt";
                filePath = Path.Combine(filePath, fileName);

                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                                                          // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(YourClassObject.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, YourClassObject);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    CloudBlockBlob cblob = container.GetBlockBlobReference(fileName);
                    cblob.UploadText(xmlDoc.InnerXml);
                }
            }
            else
            {

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);

                }
                DateTime dt = DateTime.Now;
                string fileName = dataType + "-" + dt.Day + "-" + dt.Month + "-" + dt.Year + "-";
                if (dataType == "DMAData")
                {
                    foreach (string file in Directory.GetFiles(filePath))
                    {
                        if (file.Contains(fileName))
                        {
                            File.Delete(file);
                        }
                    }
                }
                int fileCount = Directory.GetFiles(filePath, "*.txt", SearchOption.AllDirectories).Length; // Will Retrieve count of files XML extension in directry and sub directries
                fileCount = fileCount + 1;
                fileName = fileName + fileCount.ToString() + ".txt";
                filePath = Path.Combine(filePath, fileName);
                // use Path.Combine to combine 2 strings to a path

                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                                                          // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(YourClassObject.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, YourClassObject);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    File.WriteAllText(filePath, xmlDoc.InnerXml);
                }
            }
        }

        public void CacheXML(Object YourClassObject, string filePath, string dataType, bool IsAzure)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath, dataType + ".txt");

            if ((File.Exists(filePath)))
            {
                File.Delete(filePath);
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(YourClassObject.GetType());
            // Creates a stream whose backing store is memory. 
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, YourClassObject);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
                File.WriteAllText(filePath, xmlDoc.InnerXml);
            }
        }

        public Object CreateObject(Object YourClassObject, string filePath, bool IsAzure)
        {
            string XMLString = File.ReadAllText(filePath);

            XmlSerializer oXmlSerializer = new XmlSerializer(YourClassObject.GetType());
            //The StringReader will be the stream holder for the existing XML file 
            YourClassObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
            //initially deserialized, the data is represented by an object without a defined type 
            return YourClassObject;
        }
        public void CreateOptionsXML(Object YourClassObject, string filePath, string dataType, bool IsAzure)
        {
            if (IsAzure == true)
            {
                // Retrieve storage account information from connection string
                CloudStorageAccount storageAccount = BlobStorage.Common.CreateStorageAccountFromConnectionString();

                // Create a blob client for interacting with the blob service.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Create a container for organizing blobs within the storage account.
                CloudBlobContainer container = blobClient.GetContainerReference("oidata");
                container.CreateIfNotExists();

                DateTime dt = DateTime.Now;
                string fileName = dataType + "-" + dt.Day + "-" + dt.Month + "-" + dt.Year + "-";
                if (dataType == "DMAData")
                {
                    foreach (string file in Directory.GetFiles(filePath))
                    {
                        if (file.Contains(fileName))
                        {
                            File.Delete(file);
                        }
                    }
                }
                int fileCount = container.ListBlobs().Count();// Directory.GetFiles(filePath, "*.txt", SearchOption.AllDirectories).Length; // Will Retrieve count of files XML extension in directry and sub directries
                fileCount = fileCount + 1;
                fileName = fileName + fileCount.ToString() + ".txt";
                filePath = Path.Combine(filePath, fileName);

                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                                                          // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(YourClassObject.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, YourClassObject);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    CloudBlockBlob cblob = container.GetBlockBlobReference(fileName);
                    cblob.UploadText(xmlDoc.InnerXml);
                }
            }
            else
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);

                }
                DateTime dt = DateTime.Now;
                string fileName = dataType + "-" + dt.Day + "-" + dt.Month + "-" + dt.Year + "-";
                if (dataType == "DMAData")
                {
                    foreach (string file in Directory.GetFiles(filePath))
                    {
                        if (file.Contains(fileName))
                        {
                            File.Delete(file);
                        }
                    }
                }
                int fileCount = Directory.GetFiles(filePath, "*.txt", SearchOption.AllDirectories).Length; // Will Retrieve count of files XML extension in directry and sub directries
                fileCount = fileCount + 1;
                fileName = fileName + fileCount.ToString() + ".txt";
                filePath = Path.Combine(filePath, fileName);
                // use Path.Combine to combine 2 strings to a path

                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                                                          // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(YourClassObject.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, YourClassObject);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    File.WriteAllText(filePath, xmlDoc.InnerXml);
                }
            }
        }

        public Object CreateOptionsObject(Object YourClassObject, string filePath, bool IsAzure)
        {
            if (IsAzure==true)
            {
                WebClient client = new WebClient();
                string XMLString = client.DownloadString(filePath);
                XmlSerializer oXmlSerializer = new XmlSerializer(YourClassObject.GetType());
                //The StringReader will be the stream holder for the existing XML file 
                YourClassObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
                //initially deserialized, the data is represented by an object without a defined type
            }
            else
            {
                string XMLString = File.ReadAllText(filePath);
                XmlSerializer oXmlSerializer = new XmlSerializer(YourClassObject.GetType());
                //The StringReader will be the stream holder for the existing XML file 
                YourClassObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
                //initially deserialized, the data is represented by an object without a defined type 
            }
            return YourClassObject;
        }
    }
}

