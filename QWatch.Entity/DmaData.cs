using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class DmaData
    {
        public string SourceName { get; set; }

        public string FileName { get; set; }

        public string LTP { get; set; }
        public DateTime LTPDate { get; set; }

        public string FiveDMA { get; set; }
        public string TenDMA { get; set; }

        public string TwentyDMA { get; set; }

        public string ThirtyDMA { get; set; }

        public string FiftyDMA { get; set; }

        public string HundredDMA { get; set; }

        public string TwoHundredDMA { get; set; }

        public string ClosedPrice { get; set; }

    }
}
