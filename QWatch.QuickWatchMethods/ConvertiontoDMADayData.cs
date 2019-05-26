using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using Entity;
using DataContext;


namespace QuickWatchMethods
{
   public class ConvertiontoDMADayData
    {
        public List<UIDetailedDMA> ConvertOnjectToDMAData(List<Rank> data)
        {
            List<UIDetailedDMA> datalist = new List<UIDetailedDMA>();
            UIDetailedDMA objData = null;
            int count = 0;
            if ((data.Count) % 8 == 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (count == 0)
                    {
                        objData = new UIDetailedDMA();
                        objData.RankSavedTimeStamp = data[i].RankSavedTimeStamp;
                        objData.LTPDate = data[i].LTPDate;
                        objData.UniqueString = data[i].UniqueString;
                        objData.SourceName = data[i].SourceName;
                        objData.ClosedPrice= data[i].Closed;
                    }
                    if (data[i].Day == "LTP")
                    {
                        objData.LTPValue = data[i].Value;
                        objData.LTPColour = data[i].Colour;
                    }
                    else if (data[i].Day == "FiveDMA")
                    {
                        objData.FiveDMAValue = data[i].Value;
                        objData.FiveDMAColour = data[i].Colour;
                    }
                    else if (data[i].Day == "TenDMA")
                    {
                        objData.TenDMAValue = data[i].Value;
                        objData.TenDMAColour = data[i].Colour;
                    }
                    else if (data[i].Day == "TwentyDMA")
                    {
                        objData.TwentyDMAValue = data[i].Value;
                        objData.TwentyDMAColour = data[i].Colour;
                    }
                    else if (data[i].Day == "ThirtyDMA")
                    {
                        objData.ThirtyDMAValue = data[i].Value;
                        objData.ThirtyDMAColour = data[i].Colour;
                    }
                    else if (data[i].Day == "FiftyDMA")
                    {
                        objData.FiftyDMAValue = data[i].Value;
                        objData.FiftyDMAColour = data[i].Colour;
                    }
                    else if (data[i].Day == "HundredDMA")
                    {
                        objData.HundredDMAValue = data[i].Value;
                        objData.HundredDMAColour = data[i].Colour;

                    }
                    else if (data[i].Day == "TwoHundredDMA")
                    {
                        objData.TwoHundredDMAValue = data[i].Value;
                        objData.TwoHundredDMAColour = data[i].Colour;
                    }
                    count++;

                    if (count == 8)
                    {
                        datalist.Add(objData); count = 0;
                    }
                }
            }

            return datalist;
        }
    }
}
