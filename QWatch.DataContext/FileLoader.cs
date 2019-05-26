using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Entity;

namespace DataContext
{
    public class FileLoader
    {
        public DataTable GetFileHeader(out int count)
        {
            //Open	High	Low	LTP	Chng	% Chng	Volume (lacs) Turnover (crs.) 52w H	52w L	Past 365 Days	365 d % chng
            //	Past 30 Days	30 d % chng

            const int value = 15;
            count = value;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[value] { new DataColumn("Symbol", typeof(string)),
            new DataColumn("Open", typeof(string)),
            new DataColumn("High",typeof(string)),
            new DataColumn("Low",typeof(string)),
            new DataColumn("LTP",typeof(string)),
             new DataColumn("Chng",typeof(string)),
              new DataColumn("% Chng",typeof(string)),
               new DataColumn("Volume (lacs)",typeof(string)),
                new DataColumn("Turnover (crs.)",typeof(string)),
                 new DataColumn("52w H",typeof(string)),
                  new DataColumn("52w L",typeof(string)),
                 //  new DataColumn("Past 365 Days",typeof(string)),
                    new DataColumn("365 d % chng",typeof(string)),
                   //  new DataColumn("Past 30 Days",typeof(string)),
                     new DataColumn("30 Days % Change",typeof(string)),
                    new DataColumn("weightage",typeof(string)),
                  new DataColumn("Order",typeof(string))});

            return dt;
        }


        public DataTable LoadInstialData(DataTable dt,int count, string csvData, List<Weightage> weightageData,out int k)
        {
            int j = 0;
            int orderCount = 1;
            foreach (string row1 in csvData.Split('\n'))
            {

                var companyCode = row1.Split(',')[0];
                companyCode = companyCode.Replace("\"", "").Trim();
                string weightage = string.Empty;
                try
                {
                    weightage = weightageData.Where(x => x.Name.ToUpper() == companyCode.ToUpper()).ToList()[0].Value.ToString();
                }
                catch
                {
                    weightage = "100";
                }
                string row = row1 + ",\"" + weightage + "\"" + ",\" " + orderCount + "\"";

                if (!string.IsNullOrEmpty(row))
                {

                    int i = 0;
                    string name = ",\"";
                    dt.Rows.Add();
                    foreach (string cell in row.Split(new string[] { name }, StringSplitOptions.None))
                    {
                        if (j == 0)
                        {
                            dt.Rows.RemoveAt(0);
                            break;
                        }
                        if (i != count)
                        {

                            dt.Rows[dt.Rows.Count - 1][i] = cell.Replace("\"", "");
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    j++;
                }
            }
            k = j;
            return dt;
        }


        public List<string> CheckLongAndShortCount(DataTable dt)
        {
            decimal longCount = 0;
            decimal shortCount = 0;
            decimal totalCount = 0;
            List<string> objData = new List<string>();
          
            foreach (DataRow item in dt.Rows)
            {
                if (item.ItemArray[1].ToString().ToUpper() == item.ItemArray[3].ToString().ToUpper())
                {
                    longCount = longCount + Convert.ToDecimal(item.ItemArray[13]);
                }
            }

            foreach (DataRow item in dt.Rows)
            {
                if (item.ItemArray[1].ToString().ToUpper() == item.ItemArray[2].ToString().ToUpper())
                {
                    shortCount = shortCount + Convert.ToDecimal(item.ItemArray[13]);
                }
            }
            string lp = "Long Option(" + longCount.ToString() + ")";
            string sp = "Short Option(" + shortCount.ToString() + ")";

            objData.Add(lp);
            objData.Add(sp);
            return objData;
        }

        public List<string> CheckActualAndExpectedValue(DataTable dt, List<Weightage> weightageData, string keyName)
        {
            List<string> objData = new List<string>();
            string NSEValue = string.Empty;
            string SystemValue = string.Empty;
            string WCount = string.Empty;
            decimal totalCount1 = 0;


            if (keyName == "BankNifty")
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item.ItemArray[0].ToString().ToUpper() == "NIFTY BANK")
                    {
                        NSEValue = "NSE Value(" + item.ItemArray[6].ToString() + ")";
                    }
                    else
                    {
                        var companyCode = item.ItemArray[0].ToString().Split(',')[0];
                        companyCode = companyCode.Replace("\"", "").Trim();

                        if (!(String.IsNullOrEmpty(companyCode)))
                        {
                          WCount = string.Empty;
                          WCount = weightageData.Where(x => x.Name.ToUpper() == companyCode.ToUpper()).ToList()[0].Value.ToString();
                          totalCount1 = totalCount1 + (Convert.ToDecimal(WCount)) * (Convert.ToDecimal(item.ItemArray[6]));
                        }
                    }
                }
            }

            if (keyName == "Nifty50")
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item.ItemArray[0].ToString().ToUpper() == "NIFTY 50")
                    {
                        NSEValue = "NSE Value(" + item.ItemArray[6].ToString() + ")";
                    }
                    else
                    {
                        var companyCode = item.ItemArray[0].ToString().Split(',')[0];
                        companyCode = companyCode.Replace("\"", "").Trim();

                        if (!(String.IsNullOrEmpty(companyCode)))
                        {
                            WCount = string.Empty;
                            WCount = weightageData.Where(x => x.Name.ToUpper() == companyCode.ToUpper()).ToList()[0].Value.ToString();
                            totalCount1 = totalCount1 + (Convert.ToDecimal(WCount)) * (Convert.ToDecimal(item.ItemArray[6]));
                        }
                    }
                }
            }


            SystemValue = "System Value(" + totalCount1.ToString() + ")";

            objData.Add(NSEValue);
            objData.Add(SystemValue);
            return objData;           
        }

        public DataTable LoadBulkData(int orderCount,int count, List<string> csvFiles, DataTable dt, List<Weightage> weightageData, int j)
        {
            foreach (var filepath in csvFiles)
            {
                orderCount = orderCount + 1;
                string csvData = string.Empty;
                csvData = File.ReadAllText(filepath);

                foreach (string row1 in csvData.Trim().Split('\n'))
                {

                    var companyCode = row1.Split(',')[0];
                    companyCode = companyCode.Replace("\"", "").Trim();
                    string weightage = string.Empty;

                    try
                    {
                        weightage = weightageData.Where(x => x.Name.ToUpper() == companyCode.ToUpper()).ToList()[0].Value.ToString();
                    }
                    catch
                    {
                        weightage = "100";
                    }
                    string row = row1 + ",\"" + weightage + "\"" + ",\" " + orderCount + "\"";

                    if (!string.IsNullOrEmpty(row))
                    {

                        int i = 0;
                        string name = ",\"";
                        dt.Rows.Add();
                        foreach (string cell in row.Split(new string[] { name }, StringSplitOptions.None))
                        {
                            if (j == 0)
                            {
                                dt.Rows.RemoveAt(0);
                                break;
                            }
                            if (i != count)
                            {

                                dt.Rows[dt.Rows.Count - 1][i] = cell.Replace("\"", "");
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        j++;
                    }
                }
            }
            return dt;
        }

        public List<DataTable> CalcLongAndShort(DataTable dt)
        {
            List<DataTable> objData = new List<DataTable>();

            DataTable dtLong = new DataTable();
            DataTable dtShort = new DataTable();
            dtLong = dt.Clone();
            dtShort = dt.Clone();

            foreach (DataRow item in dt.Rows)
            {
                if (item.ItemArray[0].ToString().ToUpper() == "NIFTY BANK" || item.ItemArray[0].ToString().ToUpper() == "NIFTY 50")
                {
                    dtLong.Rows.Add(item.ItemArray); break;
                }
            }

            foreach (DataRow item in dt.Rows)
            {
                if (item.ItemArray[1].ToString().ToUpper() == item.ItemArray[3].ToString().ToUpper())
                {
                    dtLong.Rows.Add(item.ItemArray);
                }
            }

            foreach (DataRow item in dt.Rows)
            {
                if (item.ItemArray[1].ToString().ToUpper() == item.ItemArray[2].ToString().ToUpper())
                {
                    dtShort.Rows.Add(item.ItemArray);
                }
            }
            objData.Add(dtLong);
            objData.Add(dtShort);
            return objData;
        }
    }
}
