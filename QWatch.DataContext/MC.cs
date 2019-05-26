using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataContext
{
    public class MC
    {
        public List<MCData> LoadData()
        {
            List<MCData> objList = new List<MCData>();
            List<CompaniesList> listofCompanies = MCList.GetList();

            foreach (CompaniesList item in listofCompanies)
            {
                MCData objData = new MCData();
                string data = DataContext.BasicData.GetHtmlString(item.MainLink);
                string[] names = GetMCAlerts(data);
                List<MCActions> actions = GetMCActions(data);

                objData.Code = item.Code;
                objData.TimeStamp = DateTime.Now;
                objData.MCActions = actions;
                objData.SourceDetails = item;
                if (names != null)
                {
                    foreach (var itemSplit in names)
                    {
                        if (itemSplit.Contains('%'))
                        {
                            if (itemSplit.Contains("BUY"))
                            {
                                objData.BuyValue = Convert.ToDecimal(GetValue(itemSplit));
                            }
                            else if (itemSplit.Contains("SELL"))
                            {
                                objData.SellValue = Convert.ToDecimal(GetValue(itemSplit));
                            }
                            else if (itemSplit.Contains("HOLD"))
                            {
                                objData.HoldValue = Convert.ToDecimal(GetValue(itemSplit));
                            }
                        }
                    }
                }
                objList.Add(objData);
            }

            return objList;
        }
        private List<MCActions> GetMCActions(string data)
        {
            List<MCActions> actionsList = new List<MCActions>();
            MCActions objactions = null;

            string name2 = (((data.Split(new string[] { "marketAction" }, StringSplitOptions.None)))[1].Split(new string[] { "set_alert" }, StringSplitOptions.None))[0];
            string[] name3 = name2.Split(new string[] { "list_desc FL" }, StringSplitOptions.None);
            foreach (var item in name3)
            {
                if(item.Contains("strong") && item.Contains("timestamp"))
                {
                    objactions = new MCActions();
                    objactions.Header = item.Split(new string[] { "</strong>" }, StringSplitOptions.None)[0].Split(new string[] { "<strong>" }, StringSplitOptions.None)[1];
                    objactions.Title = item.Split(new string[] { "</a>" }, StringSplitOptions.None)[0].Split(new string[] { ">" }, StringSplitOptions.None)[item.Split(new string[] { "</a>" }, StringSplitOptions.None)[0].Split(new string[] { ">" }, StringSplitOptions.None).Length -1];
                    objactions.Date = item.Split(new string[] { "timestamp" }, StringSplitOptions.None)[1].Split(new string[] { "</span>" }, StringSplitOptions.None)[0].Split(new string[] { ">" }, StringSplitOptions.None)[1];
                    objactions.IsToday = CheckIstoday(objactions.Date);
                    actionsList.Add(objactions);
                }
            }
            return actionsList;
        }


        private bool CheckIstoday(string istoday)
        {
            bool value = false;
            if(istoday.ToLower().Contains(DateTime.Now.ToString("MMM").ToLower() + " " + DateTime.Now.Date.Day.ToString().ToLower()))
            {
                value = true;
            }
            return value;
        }
        private string[] GetMCAlerts(string data)
        {
            string[] names = null;
            if (!data.Contains("No recommendations so far."))
            {
                string name2 = (((data.Split(new string[] { "FR wd295" }, StringSplitOptions.None)))[1].Split(new string[] { "PA15" }, StringSplitOptions.None))[0];
                string name3 = name2.Split(new string[] { "PT5" }, StringSplitOptions.None)[1].Split(new string[] { "</div>" }, StringSplitOptions.None)[0];
                names = name3.Split(new string[] { "style" }, StringSplitOptions.None);
            }
            return names;
        }
        private static string GetValue(string name)
        {
            return name.Split('%')[0].Split(':')[1];
        }
    }
}
