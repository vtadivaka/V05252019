using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Rank
    {
        public int UniqueNumber { get; set; }

        public string SourceName { get; set; }

        public string Day { get; set; }

        public string Value { get; set; }

        public int RankId { get; set; }

        public DateTime LTPDate { get; set; }

        public string UniqueString { get; set; }

        public string Colour { get; set; }

        public DateTime RankSavedTimeStamp { get; set; }

        public decimal DMAScore { get; set; }

        public string Closed { get; set; }

    }
}
