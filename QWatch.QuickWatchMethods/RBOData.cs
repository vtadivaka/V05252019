using System;
using System.Collections.Generic;
using System.Linq;
using ObjToXml;
using System.IO;
using System.Configuration;
using Entity;

namespace QuickWatchMethods
{
  public class RBOData
    {
        private List<string> FilterDates(List<string> files, string _defaultDate)
        {
            List<string> list = new List<string>();
            bool value = false;
            foreach (var item in files)
            {
                if (value == true)
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
        private bool checkIsValidFileForRBO(string currentFile)
        {
            bool value = false;
            FileInfo fi = new FileInfo(currentFile);
            DateTime lastmodified = fi.LastWriteTime;
            string day = lastmodified.DayOfWeek.ToString();

            TimeSpan start = new TimeSpan(9, 15, 0); //10 o'clock
            TimeSpan end = new TimeSpan(17, 30, 0); //12 o'clock
            TimeSpan now = lastmodified.TimeOfDay;

            if (day != "Sunday" || day != "Saturday")
            {
                if ((now > start) && (now < end))
                {
                    value = true;
                }
            }
            return value;
        }
        public List<RBOEntity> RBO(string _defaultDate)
        {
            List<RBOEntity> objRBOEntityList = new List<RBOEntity>();
            string _masterDatapathBasic = ConfigurationManager.AppSettings["MasterDatapathBasic"];
            string _masterDatapathDMA = ConfigurationManager.AppSettings["MasterDatapathDMA"];

            BulkData obj = new BulkData();
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.BasicData = new List<CompanyDetails>();
            int RBOcount = 173;
            string[] txtFiles = Directory.GetFiles(_masterDatapathBasic, "*.txt*", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();

            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                if (checkIsValidFileForRBO(currentFile))
                {
                    datafromXML.BasicData.AddRange((List<CompanyDetails>)xmltoObj.CreateObject(datafromXML.BasicData, currentFile, false));
                    if (RBOcount == datafromXML.BasicData.Count)
                    {
                        break;
                    }
                    else
                    {
                        datafromXML.BasicData = new List<CompanyDetails>();
                    }
                }
            }

            //test to read data
            datafromXML.DMAData = new List<Rank>();
            var txtDMAFiles = Directory.EnumerateFiles(_masterDatapathDMA, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            foreach (string currentFile in txtDMAFiles)
            {
                if (checkIsValidFileForRBO(currentFile))
                {
                    datafromXML.DMAData.AddRange((List<Rank>)xmltoObj.CreateObject(datafromXML.DMAData, currentFile, false));
                    break;
                }
            }

            RBOEntity objEntity;
            foreach (var basicData in datafromXML.BasicData.Where(x => x.Code != null))
            {
                objEntity = new RBOEntity();
                objEntity.Code = basicData.Code;
                objEntity.LTPValue = basicData.lastPrice;
                objEntity.LTPDate = basicData.BasicSavedTimeStamp;
                objEntity.low52 = basicData.low52;
                objEntity.high52 = basicData.high52;
                objEntity.TotalVolume = basicData.TotalVolume;
                objEntity.CurrentPrevdayVolumePercentage = basicData.CurrentPrevdayVolumePercentage;

                objEntity.BOPercentage = Math.Round(((Convert.ToDecimal(objEntity.LTPValue) * 100) / Convert.ToDecimal(objEntity.high52)),2).ToString();
                var dmaData = datafromXML.DMAData.Where(x => x.SourceName == basicData.Code).ToList();
                decimal rboValue = 0.0M;
                decimal ltpValue = 0.0M;
                int count = 0;
                foreach (var itemDma in dmaData)
                {
                    if (itemDma.Day != "LTP")
                    {
                        rboValue = rboValue + Convert.ToDecimal(itemDma.Value); count++;
                    }
                    else
                    {
                        ltpValue = Convert.ToDecimal(itemDma.Value);
                    }
                }
                decimal rboAvgValue = rboValue / count;
                decimal rboPercentage = ((ltpValue - rboAvgValue) * 100) / ltpValue;
                objEntity.RangeBoundPercentage = Math.Round(rboPercentage,2).ToString();
                objRBOEntityList.Add(objEntity);
            }
            return objRBOEntityList;
        }
    }
}
