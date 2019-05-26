using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace ObjToXml
{
    public class OBJtoXML
    {

        public void CreateOptionsXML(Object YourClassObject, string filePath, string dataType)
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

        public Object CreateOptionsObject(Object YourClassObject, string filePath)
        {
            string XMLString = File.ReadAllText(filePath);

            XmlSerializer oXmlSerializer = new XmlSerializer(YourClassObject.GetType());
            //The StringReader will be the stream holder for the existing XML file 
            YourClassObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
            //initially deserialized, the data is represented by an object without a defined type 
            return YourClassObject;
        }
    }
}

