using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataContext
{
    public static class MCList
    {
        public static List<CompaniesList> GetList()
        {
            List<CompaniesList> objlist = new List<CompaniesList>();
            CompaniesList obj = new CompaniesList();


            obj.Code = "INDUSINDBK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/indusindbank/IIB";
            obj.Category = "BN";
            obj.CommentsLink = "http://mmb.moneycontrol.com/stock-message-forum/indusindbank/comments/666";
            objlist.Add(obj);

            obj = new CompaniesList();
            obj.Code = "FEDERALBNK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/federalbank/FB";
            obj.Category = "BN";
            obj.CommentsLink = "http://mmb.moneycontrol.com/stock-message-forum/federalbank/comments/6423";
            objlist.Add(obj);

            obj = new CompaniesList();
            obj.Code = "KOTAKBANK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/kotakmahindrabank/KMB";
            obj.Category = "BN";
            obj.CommentsLink = "http://mmb.moneycontrol.com/stock-message-forum/kotakmahindrabank/comments/3441";
            objlist.Add(obj);


            obj = new CompaniesList();
            obj.Code = "HDFCBANK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/hdfcbank/HDF01";
            obj.Category = "BN";
            obj.CommentsLink = "http://mmb.moneycontrol.com/stock-message-forum/hdfcbank/comments/4962";
            objlist.Add(obj);

            obj = new CompaniesList();
            obj.Code = "BANKBARODA";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-public-sector/bankofbaroda/BOB";
            obj.Category = "BN";
            obj.CommentsLink = "http://mmb.moneycontrol.com/stock-message-forum/bankofbaroda/comments/261";
            objlist.Add(obj);

            obj = new CompaniesList();
            obj.Code = "SBIN";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-public-sector/statebankindia/SBI";
            obj.Category = "BN";
            obj.CommentsLink = "";
            objlist.Add(obj);


            obj = new CompaniesList();
            obj.Code = "IDFCBANK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/idfcbank/IDF01";
            obj.Category = "BN";
            obj.CommentsLink = "";
            objlist.Add(obj);


            obj = new CompaniesList();
            obj.Code = "ICICIBANK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/icicibank/ICI02";
            obj.Category = "BN";
            obj.CommentsLink = "";
            objlist.Add(obj);

            obj = new CompaniesList();
            obj.Code = "CANBK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-public-sector/canarabank/CB06";
            obj.Category = "BN";
            obj.CommentsLink = "";
            objlist.Add(obj);

            obj = new CompaniesList();
            obj.Code = "PNB";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-public-sector/punjabnationalbank/PNB05";
            obj.Category = "BN";
            obj.CommentsLink = "";
            objlist.Add(obj);


            obj = new CompaniesList();
            obj.Code = "AXISBANK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/axisbank/AB16";
            obj.Category = "BN";
            obj.CommentsLink = "";
            objlist.Add(obj);


            obj = new CompaniesList();
            obj.Code = "YESBANK";
            obj.MainLink = "http://www.moneycontrol.com/india/stockpricequote/banks-private-sector/yesbank/YB";
            obj.Category = "BN";
            obj.CommentsLink = "";
            objlist.Add(obj);

            return objlist;
        }
    }
}
