using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OIOData;

namespace Entity
{
    public class QueryBinder
    {
        public List<OIDetails> CallOptionsCurrentBNData { get; set; }
        public List<OIDetails> PutOptionsCurrentBNData { get; set; }

        public List<OIDetails> CallOptionsNextBNData { get; set; }
        public List<OIDetails> PutOptionsNextBNData { get; set; }

        public List<OIDetails> CallOptionsNData { get; set; }
        public List<OIDetails> PutOptionsNData { get; set; }

        public List<OIDetails> StkPutData { get; set; }

        public List<OIDetails> StkCallData { get; set; }

    }
}