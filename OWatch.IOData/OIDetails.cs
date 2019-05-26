using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OIOData
{
    public class OIDetails
    {
        public string lastPrice { get; set; }
        public string prevClose { get; set; }
        public string openPrice { get; set; }
        public string highPrice { get; set; }
        public string lowPrice { get; set; }
        public string closePrice { get; set; }
        public string vwap { get; set; }
        public string underlyingValue { get; set; }
        public string changeinOpenInterest { get; set; }
        public string openInterest { get; set; }
        public string pchangeinOpenInterest { get; set; }

        public string optionType { get; set; }
        public string strikePrice { get; set; }
        public string instrumentType { get; set; }
        public string expiryDate { get; set; }

        public string underlying { get; set; }
        public DateTime SavedTimeStamp { get; set; }
        public string BackColor { get; set; }

        public string TotalCount { get; set; }
    }
    public class Types
    {
        public float Type { get; set; }
        public float Currentvalue { get; set; }
        public List<float> requiredValues { get; set; }
       
    }
}
