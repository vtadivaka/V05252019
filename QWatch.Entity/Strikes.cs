using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class Strikes
    {
        //Never change the Order of the below properties
        public string Cal_OI { get; set; }
        public string Cal_ChnginOI { get; set; }
        public string Cal_Volume { get; set; }
        public string Cal_IV { get; set; }
        public string Cal_LTP { get; set; }
        public string Cal_NetChng { get; set; }
        public string Cal_BidQty { get; set; }
        public string Cal_BidPrice { get; set; }
        public string Cal_AskPrice { get; set; }
        public string Cal_AskQty { get; set; }

        public string StrikePrice { get; set; }

        public string Put_BidQty { get; set; }
        public string Put_BidPrice { get; set; }
        public string Put_AskPrice { get; set; }
        public string Put_AskQty { get; set; }
        public string Put_NetChng { get; set; }
        public string Put_LTP { get; set; }
        public string Put_IV { get; set; }
        public string Put_Volume { get; set; }
        public string Put_ChnginOI { get; set; }
        public string Put_OI { get; set; }
        public string Time { get; set; }
    }
}
