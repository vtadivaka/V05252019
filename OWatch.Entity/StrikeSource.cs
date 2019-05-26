using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{

    public static class StrikeSource
    {
        public static List<SourceList> Sources(string bnSource, string nsource,string nexpiry, string bnexpiry, bool isLoadAll)
        {
            List<SourceList> Obj = new List<SourceList>();
            SourceList sources = null;
            WeightageClass obj1 = new WeightageClass();
            List<Weightage> weightageDataNifty50 = new List<Weightage>();
            weightageDataNifty50 = obj1.WeightageBNData(nsource);
            List<Weightage> weightageDataBankNifty = new List<Weightage>();
            weightageDataBankNifty = obj1.WeightageBNData(bnSource);


            if (isLoadAll)
            {
                //Adding values for stock
                foreach (var item in weightageDataNifty50)
                {
                    sources = new SourceList();
                    var value = weightageDataBankNifty.Where(x => x.Name.ToUpper() == item.Name.ToUpper()).ToList();
                    if (value.Count() > 0)
                    {
                        sources.WeightageBN = value[0].Value;
                    }
                    sources.SourceName = item.Name;
                    sources.WeightageN = item.Value;
                    sources.date = nexpiry;
                    sources.symbol = item.Name;
                    sources.instrument = "OPTSTK";
                    Obj.Add(sources);
                }
            }
            //Adding values for Index
            sources = new SourceList();
            sources.SourceName = "NIFTY";
            sources.WeightageN = "";
            sources.date = nexpiry;
            sources.symbol = "NIFTY";
            sources.instrument = "OPTIDX";
            Obj.Add(sources);

            sources = new SourceList();
            sources.SourceName = "BANKNIFTY";
            sources.WeightageN = "";
            sources.date = nexpiry;
            sources.symbol = "BANKNIFTY";
            sources.instrument = "OPTIDX";
            Obj.Add(sources);

            sources = new SourceList();
            sources.SourceName = "BANKNIFTY";
            sources.WeightageN = "";
            sources.date = bnexpiry;
            sources.symbol = "BANKNIFTY";
            sources.instrument = "OPTIDX";
            Obj.Add(sources);


            return Obj;
        }
    }
}
