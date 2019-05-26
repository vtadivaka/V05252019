using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public class OptionChain
    {
        public string TimeStamp { get; set; }
        public string SourceName { get; set; }
        public string SourceValue { get; set; }
        public string ExpiryDate { get; set; }
        public Int64 total_cal_OI { get; set; }
        public Int64 total_cal_ChnginOI { get; set; }
        public Int64 total_cal_Volume { get; set; }
        public Int64 total_put_OI { get; set; }
        public Int64 total_put_ChnginOI { get; set; }
        public Int64 total_put_Volume { get; set; }
        public string SourceType { get; set; }

        public string Percentage { get; set; }
        public List<Strikes> Strikes { get; set; }
        public string UnderlyingValue { get; set; }

        public string AsOnTime { get; set; }
       
    }

    public class OptionChain2
    {
        public string TimeStamp { get; set; }
        public string SourceName { get; set; }
        public string SourceValue { get; set; }
        public string ExpiryDate { get; set; }
        public string total_cal_OI { get; set; }
        public string total_cal_ChnginOI { get; set; }
        public string total_cal_Volume { get; set; }
        public string total_put_OI { get; set; }
        public string total_put_ChnginOI { get; set; }
        public string total_put_Volume { get; set; }
        public string SourceType { get; set; }

        public string Percentage { get; set; }
        public List<Strikes> Strikes { get; set; }
        public string UnderlyingValue { get; set; }

        public string AsOnTime { get; set; }
    }
}
