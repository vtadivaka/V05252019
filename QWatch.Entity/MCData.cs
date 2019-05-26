using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   
        public class MCData
        {
            public decimal BuyValue { get; set; }

            public decimal SellValue { get; set; }

            public decimal HoldValue { get; set; }

            public string Code { get; set; }
            public string MainLink { get; set; }
            public string CommentsLink { get; set; }

            public DateTime TimeStamp { get; set; }

            public List<MCActions> MCActions { get; set; }

            public CompaniesList SourceDetails { get; set; }


        }

}
