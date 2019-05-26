using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public class UIDetailedDMA
    {
        public DateTime RankSavedTimeStamp { get; set; }
        public DateTime LTPDate { get; set; }
        public string UniqueString { get; set; }
        public string SourceName { get; set; }

        public string LTPValue { get; set; }
        public string LTPColour { get; set; }

        public string FiveDMAValue { get; set; }
        public string FiveDMAColour { get; set; }


        public string TenDMAValue { get; set; }
        public string TenDMAColour { get; set; }

        public string TwentyDMAValue { get; set; }
        public string TwentyDMAColour { get; set; }

        public string ThirtyDMAValue { get; set; }
        public string ThirtyDMAColour { get; set; }

        public string FiftyDMAValue { get; set; }
        public string FiftyDMAColour { get; set; }


        public string HundredDMAValue { get; set; }
        public string HundredDMAColour { get; set; }

        public string TwoHundredDMAValue { get; set; }
        public string TwoHundredDMAColour { get; set; }

        public string ItemBasedChange { get; set; }

        public decimal IndexScore { get; set; }
        public decimal ChangeScore { get; set; }

        public string ClosedPrice { get; set; }

    }
}
