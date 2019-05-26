using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OIOData;
using Entity;
using System.Configuration;
using OptionChainData;
using ObjToXml;
using System.IO;

namespace ConsoleApplication1
{

class Program
    {
        static void Main(string[] args)
        {
            OptionChainClass obj = new OptionChainClass();

            string[] dates= { "25OCT2018", "11OCT2018" };
            obj.LoadFullOptionChainData(@"D:\Analyzer\MasterData\FullOptionsData\", dates,false);

            OBJtoXML xmltoObj = new OBJtoXML();
            string[] txtFiles = Directory.GetFiles(@"D:\Analyzer\MasterData\FullOptionsData\", "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            //var txtFiles = Directory.EnumerateFiles("D:\\MasterData\\Basic\\", " *.txt").OrderByDescending(x => x);
            List<OptionChain> optionsData = new List<OptionChain>();
            foreach (string currentFile in txtFiles)
            {
                optionsData.AddRange((List<OptionChain>)xmltoObj.CreateOptionsObject(optionsData, currentFile));
            }
            optionsData = optionsData.Where(x => x.SourceName == "NIFTY" || x.SourceName == "BANKNIFTY").ToList();
            optionsData = optionsData.OrderByDescending(x => x.TimeStamp).ToList();

            // optionsData=optionsData.OrderByDescending(x => Convert.ToDecimal(x.Percentage)).ToList();


        }



    }
}
