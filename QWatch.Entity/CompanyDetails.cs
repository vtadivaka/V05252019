using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class CompanyDetails
    {

        public string CompanyName { get; set; }
        public string Code { get; set; }
        public string PreClose { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Closed { get; set; }
        public string TotalVolume { get; set; }
        public string DeliveryVolume { get; set; }
        public string Delivery { get; set; }
        public string Date { get; set; }
        public string UpdateToday { get; set; }
        public string pChange { get; set; }
        public string change { get; set; }
        public string low52 { get; set; }
        public string high52 { get; set; }
        public string totalBuyQuantity { get; set; }
        public string totalSellQuantity { get; set; }
        public DateTime BasicSavedTimeStamp { get; set; }
        public string Percentage { get; set; }
        public string lastPrice { get; set; }

        public string CalcDayVolume { get; set; }
        public string CurrentPrevdayVolumePercentage { get; set; }

        public string fastPercentage { get; set; }
        public string BNWeigtage { get; set; }
        public int Id { get; set; }
        //public string lastPrice { get; set; }
        //public string lastPrice { get; set; }
        //public string lastPrice { get; set; }

    }
}
