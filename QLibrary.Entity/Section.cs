using System;
using System.Collections.Generic;
using System.Text;

namespace EntityData
{
  public  class Section
    {
        public int SectionId { get; set; }
        public int RequiredCount { get; set; }
        public int TotalCount { get; set; }
        public string SectionName { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Section> SectionDetails { get; set; }
    }
}
  