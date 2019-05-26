using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class RBOEntity
    {  //minmunim days are required to calculate this.
        public int UniqueId { get; set; }
        public string Code { get; set; }
        public string LTPValue { get; set; }
        public DateTime LTPDate { get; set; }
        public string low52 { get; set; }
        public string high52 { get; set; }
        public string BOPercentage { get; set; }
        //public string LastBOValue { get; set; }
        // public string LastBOTime { get; set; }
        public string RangeBoundPercentage { get; set; }
        public string TotalVolume { get; set; }
        public string CurrentPrevdayVolumePercentage { get; set; }
        public string RBOHistoryString { get; set; }

    }
}
