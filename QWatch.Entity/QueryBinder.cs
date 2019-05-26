using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entity
{
    public class QueryBinder
    {

        public List<CompanyDetails> PosativeData { get; set; }
        public List<CompanyDetails> NegativeData { get; set; }
        public List<CompanyDetails> HighBuying { get; set; }
        public List<CompanyDetails> HighSelling { get; set; }
        public List<OIDetails> CheckPriceStrongOIStroing { get; set; }
        public List<OIDetails> CheckPriceWeekOIStroing { get; set; }
        public List<OIDetails> CheckPriceStrongOIWeek { get; set; }
        public List<OIDetails> CheckPriceWeekIOWeek { get; set; }
        public List<Rank> CheckLTPHasFirstPlace { get; set; }
        public List<Rank> CheckLTPHasSecondPlace { get; set; }
        public List<Rank> CheckLTPHasLastPlace { get; set; }
        public List<Rank> CheckLTPHasBeforeLastPlace { get; set; }
        public List<Rank> NiftyDMAData { get; set; }
        public List<Rank> BankNiftyDMAData { get; set; }
        public List<UIDetailedDMA> ChangeDMAPosation { get; set; }
        public List<UIDetailedDMA> UIDetailedDMA { get; set; }

        public List<RBOEntity> RBOData { get; set; }

        public List<DiscountEntity> DiscountData { get; set; }
        public List<AvgVolums> AvgVolums { get; set; }
        public List<MCData> MCData { get; set; }













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