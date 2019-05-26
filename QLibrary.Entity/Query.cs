using System;
using System.Collections.Generic;
using System.Text;

namespace EntityData
{
   public class Query
    {  
        public int QueryId { get; set; }

        public int SectionId { get; set; }

        public string SectionName { get; set; }

        public string QueryName { get; set; }

        public string Description { get; set; }

        public DateTime SCreatedDate { get; set; }
        public List<Section> SectionDetails { get; set; }
    }
}
