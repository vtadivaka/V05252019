using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjToXml;
using Entity;
using DataContext;
using System.IO;
using System.Collections;

namespace QuickWatchMethods
{
    public class OILogic
    {
        private List<string> FilterDates(List<string> files, string _defaultDate)
        {
            List<string> list = new List<string>();
            bool value = false;
            foreach (var item in files)
            {
                if (value == true)
                {
                    list.Add(item);
                }
                else
                {
                    if (item.Trim().ToLower().Contains(_defaultDate.Trim().ToLower()))
                    {
                        value = true;
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        public List<OIDetails> CheckPriceStrongOIStroing(string _masterDatapathBasic, string _defaultDate)
        {
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.OIData = new List<OIDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.OIData.AddRange((List<OIDetails>)xmltoObj.CreateObject(datafromXML.OIData, currentFile, false)); break;
            }

            var query8 = datafromXML.OIData.Where(x => x.PchangeinOpenInterest != null && x.PChange != null && x.PchangeinOpenInterest != "-" && x.PChange != "-")
                .Where(x => Convert.ToDecimal(x.PChange) > 0 && Convert.ToDecimal(x.PchangeinOpenInterest) > 0)
                                             .Select(x =>
                                             new
                                             {
                                                 Code = x.CompanyCode,
                                                 x.PChange,
                                                 x.PchangeinOpenInterest,
                                             })
                                              .OrderByDescending(x => x.PchangeinOpenInterest)
                                             .ToList();

            List<OIDetails> newlist = new List<OIDetails>();
            OIDetails cmp = null;
            foreach (var item in query8)
            {
                cmp = new OIDetails();
                cmp.CompanyCode = item.Code;
                cmp.PChange = item.PChange.ToString();
                cmp.PchangeinOpenInterest = item.PchangeinOpenInterest;
                newlist.Add(cmp);
            }

            return newlist;

        }

        public List<OIDetails> CheckPriceWeekIOWeek(string _masterDatapathBasic, string _defaultDate)
        {
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.OIData = new List<OIDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.OIData.AddRange((List<OIDetails>)xmltoObj.CreateObject(datafromXML.OIData, currentFile, false)); break;
            }

            var query7 = datafromXML.OIData.Where(x => x.PchangeinOpenInterest != null && x.PChange != null && x.PchangeinOpenInterest != "-" && x.PChange != "-")
               .Where(x => Convert.ToDecimal(x.PChange) < 0 && Convert.ToDecimal(x.PchangeinOpenInterest) < 0)
                                            .Select(x =>
                                            new
                                            {
                                                Code = x.CompanyCode,
                                                x.PChange,
                                                x.PchangeinOpenInterest,
                                            })
                                             .OrderByDescending(x => x.PchangeinOpenInterest)
                                            .ToList();

            List<OIDetails> newlist = new List<OIDetails>();
            OIDetails cmp = null;
            foreach (var item in query7)
            {
                cmp = new OIDetails();
                cmp.CompanyCode = item.Code;
                cmp.PChange = item.PChange.ToString();
                cmp.PchangeinOpenInterest = item.PchangeinOpenInterest;
                newlist.Add(cmp);
            }

            return newlist;

        }

        public List<OIDetails> CheckPriceStrongOIWeek(string _masterDatapathBasic, string _defaultDate)
        {
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.OIData = new List<OIDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.OIData.AddRange((List<OIDetails>)xmltoObj.CreateObject(datafromXML.OIData, currentFile, false)); break;
            }

            var query6 = datafromXML.OIData.Where(x => x.PchangeinOpenInterest != null && x.PChange != null && x.PchangeinOpenInterest != "-" && x.PChange != "-")
               .Where(x => Convert.ToDecimal(x.PChange) > 0 && Convert.ToDecimal(x.PchangeinOpenInterest) < 0)
                                            .Select(x =>
                                            new
                                            {
                                                Code = x.CompanyCode,
                                                x.PChange,
                                                x.PchangeinOpenInterest,
                                            })
                                             .OrderByDescending(x => x.PchangeinOpenInterest)
                                            .ToList();

            List<OIDetails> newlist = new List<OIDetails>();
            OIDetails cmp = null;
            foreach (var item in query6)
            {
                cmp = new OIDetails();
                cmp.CompanyCode = item.Code;
                cmp.PChange = item.PChange.ToString();
                cmp.PchangeinOpenInterest = item.PchangeinOpenInterest;
                newlist.Add(cmp);
            }

            return newlist;

        }

        public List<OIDetails> CheckPriceWeekOIStroing(string _masterDatapathBasic, string _defaultDate)
        {
            ////test to read data
            OBJtoXML xmltoObj = new OBJtoXML();
            BulkEntity datafromXML = new BulkEntity();
            datafromXML.OIData = new List<OIDetails>();
            var txtFiles = Directory.EnumerateFiles(_masterDatapathBasic, "*.txt").OrderByDescending(d => new FileInfo(d).LastWriteTime).ToArray();
            List<string> filesList = FilterDates(txtFiles.ToList(), _defaultDate);
            foreach (string currentFile in filesList)
            {
                datafromXML.OIData.AddRange((List<OIDetails>)xmltoObj.CreateObject(datafromXML.OIData, currentFile, false)); break;
            }

            var query5 = datafromXML.OIData.Where(x => x.PchangeinOpenInterest != null && x.PChange != null && x.PchangeinOpenInterest != "-" && x.PChange != "-")
               .Where(x => Convert.ToDecimal(x.PChange) < 0 && Convert.ToDecimal(x.PchangeinOpenInterest) > 0)
                                            .Select(x =>
                                            new
                                            {
                                                Code = x.CompanyCode,
                                                x.PChange,
                                                x.PchangeinOpenInterest,
                                            })
                                             .OrderByDescending(x => x.PchangeinOpenInterest)
                                            .ToList();

            List<OIDetails> newlist = new List<OIDetails>();
            OIDetails cmp = null;
            foreach (var item in query5)
            {
                cmp = new OIDetails();
                cmp.CompanyCode = item.Code;
                cmp.PChange = item.PChange.ToString();
                cmp.PchangeinOpenInterest = item.PchangeinOpenInterest;
                newlist.Add(cmp);
            }

            return newlist;

        }
    }
}
