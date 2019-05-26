using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class DiscountEntity
    {
        public string DiscountPrice { get; set; }
        public string CompanyCode { get; set; }


        public string IOLtp { get; set; }
        public string IOPreviousClose { get; set; }
        public string IOOpenPrice { get; set; }
        public string IOLowPrice { get; set; }
        public string IOHighPrice { get; set; }
        public string IOPercentChange { get; set; }
        public string IOClosed { get; set; }


        public string BasicLtp { get; set; }
        public string BasicPreviousClose { get; set; }
        public string BasicOpenPrice { get; set; }
        public string BasicLowPrice { get; set; }
        public string BasicHighPrice { get; set; }
        public string BasicPercentChange { get; set; }
        public string BasicClosed { get; set; }

        public bool IsBasicMaxValuePreviousClose { get; set; }
        public bool IsBasicMaxValueClose { get; set; }
        public string ExpectedChange { get; set; }
    }
}
