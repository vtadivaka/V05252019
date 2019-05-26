using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class AvgByDate
    {
        public string TwoVolume { get; set; }
        public string TwoOIVolume { get; set; }

        public string TwoVolumePercentage { get; set; }
        public string TwoOIVolumePercentage { get; set; }


        public string FiveVolume { get; set; }
        public string FiveOIVolume { get; set; }

        public string FiveVolumePercentage { get; set; }
        public string FiveOIVolumePercentage { get; set; }


        public string TenVolume { get; set; }
        public string TenOIVolume { get; set; }

        public string TenVolumePercentage { get; set; }
        public string TenOIVolumePercentage { get; set; }
    }
    public  class AvgVolums
    {
        public string Code { get; set; }
        public DateTime LTPDate { get; set; }
        public bool IsBasicBuy { get; set; }
        public bool IsOIBuy { get; set; }
        public string BasicPercentage { get; set; }
        public string IOPercentage { get; set; }
        public string DynamicList { get; set; }
        public string TotalVolume { get; set; }
        public string OITotalVolume { get; set;}        

        public string TotalVolumePercentage { get; set; }
        public string OITotalVolumePercentage { get; set; }

        public AvgByDate AvgByDate { get; set; }
    }
}
