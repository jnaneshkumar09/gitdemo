using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Blockchaninaccounts.App_Start
{
    public class Notification
    {
        ValueController ob1 = new ValueController();
        public EmailNotification GetNotification(string id)
        {
            //ValueController ob1 = new ValueController();
            DataSet ds = ob1.GetNotification(id);
            // Session["Notification"] = ds;
            EmailNotification en = new EmailNotification();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                en.send = (bool)dr[0];
                en.Receive = (bool)dr[1];
                en.Buy = (bool)dr[2];
                en.Sell = (bool)dr[3];
                en.Exchange = (bool)dr[4];

            }
            //Session["Notification"] = en;
            return en;
        }
      
        public List<CoinList> GetCoin()
        {
            ValueController ob1 = new ValueController();
            DataSet ds = ob1.GetCoin();

            List<CoinList> Coinlist = new List<CoinList>();

            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                Coinlist.Add(new CoinList { CoinName = dr["CoinName"].ToString(), CoinId = Convert.ToInt32(dr["Id"]) });

            }
            return Coinlist;
        }
    }
}