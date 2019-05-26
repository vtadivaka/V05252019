using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entity
{
   public class BulkEntity
    {
        public List<Rank> DMAData { get; set; }
        public List<OIDetails> OIData { get; set; }
        public List<CompanyDetails> BasicData { get; set; }
    }
}
