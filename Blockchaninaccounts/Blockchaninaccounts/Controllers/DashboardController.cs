using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blockchaninaccounts.Models;
using BittrexApi;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Core.Objects;
using System.Collections;
using System.Web.Helpers;
using System.Web.UI.DataVisualization;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.App_Start;

namespace Blockchaninaccounts.Controllers
{
 [Session]
 [Authorize]
    public class DashboardController : Controller
    {
        BlockChainEntities db = new BlockChainEntities();
        
        public ActionResult Index()
        {
            decimal? mark101 = 0;
            decimal? mark102 = 0;
            decimal? mark103 = 0;
            decimal? mark104 = 0;
            decimal? mark105 = 0;
            decimal? mark106 = 0;
            //decimal? markup = 0;
            var d = CoinsMarketValueController.GetMarkupWithCoinId();
            foreach (var r in d)
            {
                switch(r.Key)
                {
                    case 101:mark101 = r.Value; break;
                    case 102:mark102 = r.Value; break;
                    case 103:mark103 = r.Value; break;
                    case 104:mark104 = r.Value; break;
                    case 105:mark105 = r.Value; break;
                    case 106:mark106 = r.Value; break;
                }
            }

            var userId = User.Identity.GetUserId();
            var transaction = db.GetRecentTransactionList(userId);
            ViewBag.userid = userId;
            var activity = db.GetActivityLogList(userId);
            var walletbalance = db.GetWalletBalance(userId);
            var coinlist = db.GetCoinsList();

            ViewBag.totaltransaction = db.GetTotalTransaction(userId).DefaultIfEmpty(0).FirstOrDefault();  //.SingleOrDefault();
            decimal Totalbalance=0;
            decimal availablebalance=0;

            foreach (GetWalletBalance_Result result in walletbalance)
            {
                if (result.CoinName == "MicroBitcoin")   
                {
                    Totalbalance = Totalbalance + Convert.ToDecimal(result.TotalBalance);
                    availablebalance = availablebalance + Convert.ToDecimal(result.AvailableBalance);
                }
            }
            ViewBag.Totalbalance = Totalbalance;
            ViewBag.availablebalance = availablebalance;


            walletbalance = db.GetWalletBalance(userId);
            

            APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();

            try { ViewBag.BTC = "$" + Math.Round((Convert.ToDecimal(objCoin.BTCCurrentPrice())+ Convert.ToDecimal(mark101)), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
            try { ViewBag.ETH = "$" + Math.Round((Convert.ToDecimal(objCoin.ETHCurrentPrice())+ Convert.ToDecimal(mark106)), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
            try { ViewBag.DASH = "$" + Math.Round((Convert.ToDecimal(objCoin.DASHCurrentPrice())+Convert.ToDecimal(mark102)),2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
            try { ViewBag.LTC = "$" + Math.Round((Convert.ToDecimal(objCoin.LTCCurrentPrice())+ Convert.ToDecimal(mark103)), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
            try { ViewBag.ETC = "$" + Math.Round((Convert.ToDecimal(objCoin.ETCCurrentPrice())+ Convert.ToDecimal(mark105)), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
            try { ViewBag.MBC = "$" + Math.Round((Convert.ToDecimal(objCoin.GetMBC_USDCoin())+ Convert.ToDecimal(mark104)), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

            ViewBag.activity = activity;
            ViewBag.walletbalance = walletbalance;
            ViewBag.coinlist = coinlist;

            string username = (string)Session["UserName"];
            SessionTokenGenerationcs Sg = new SessionTokenGenerationcs();
            UserSessionModel USobj = new UserSessionModel();
            var Token1 = Sg.Token(USobj);
            if (Token1 == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(transaction);
        }

       public ActionResult GetBalances(string ddlvalue,string userId)  
        {
            var walletbalance = db.GetWalletBalance(userId);

            foreach (GetWalletBalance_Result result in walletbalance)
            {
                if (ddlvalue.Trim() == result.CoinName)
                {
                    var coinid = (from x in db.tbl_Coin_Master where x.CoinName == result.CoinName select x).FirstOrDefault();
                    var test = MyChart1(coinid.Id.ToString(), userId);
                    string imreBase64Data = Convert.ToBase64String(test.FileContents);
                    string imgDataURL1 = string.Format("data:image1/png;base64,{0}", imreBase64Data);
                    result.ImageChart = imgDataURL1;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false });
        }

        public FileContentResult MyChart1(string coinid, string userId)
        {
            int coinid1 = Convert.ToInt32(coinid);
            IEnumerable<GetTotalBalance_Result> TotalbalanceResult = from result in db.GetTotalBalance(userId, coinid1).ToList()
                                                                     select new GetTotalBalance_Result
                                                                     {
                                                                         Transtype = result.Transtype,
                                                                         balance = result.balance
                                                                     };

            List<GetTotalBalance_Result> asList = TotalbalanceResult.ToList();

            ArrayList transtype = new ArrayList();
            ArrayList balance = new ArrayList();

            foreach (GetTotalBalance_Result obj in asList)
            {
                transtype.Add(obj.Transtype);
                balance.Add(obj.balance);
            }

            var bytes = new System.Web.Helpers.Chart(width: 350, height: 350, theme: ChartTheme.Blue)
                     .AddSeries(
                     chartType: "pie",
                     xValue: transtype,
                     yValues: balance)
                     .GetBytes("png");

            return File(bytes, "image/png");
        }
        public ActionResult MyChart()
     	{
            var userId = User.Identity.GetUserId();
            int coinid1 = 104;  
            IEnumerable<GetTotalBalance_Result> TotalbalanceResult = from result in db.GetTotalBalance(userId, coinid1).ToList()  
                                                                     select new GetTotalBalance_Result
                                                                     {
                                                                         Transtype = result.Transtype,
                                                                         balance = result.balance
                                                                     };

            List<GetTotalBalance_Result> asList = TotalbalanceResult.ToList();

            ArrayList transtype = new ArrayList();
            ArrayList balance = new ArrayList();

            foreach (GetTotalBalance_Result obj in asList)
            {
                transtype.Add(obj.Transtype);
                balance.Add(obj.balance);
            }

       var bytes = new System.Web.Helpers.Chart(width: 350, height: 350, theme: ChartTheme.Blue)
                .AddSeries( 
                chartType: "pie", 
                xValue: transtype,
	            yValues: balance)
    	        .GetBytes("png");
               
	    return File(bytes, "image/png");
	}


      



    }
}
