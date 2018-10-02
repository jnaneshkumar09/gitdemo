using Blockchaninaccounts.App_Start;
using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Blockchaninaccounts.Controllers.Dashboard
{
    [Session]
    [Authorize]
    public class SendController : BaseController
    {
        decimal? mark101 = 0;
        decimal? mark102 = 0;
        decimal? mark103 = 0;
        decimal? mark104 = 0;
        decimal? mark105 = 0;
        decimal? mark106 = 0;
        Dictionary<int, decimal?> d = CoinsMarketValueController.GetMarkupWithCoinId();

        APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
        string url = ConfigurationManager.AppSettings["Url"];
        // GET: Send
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendBitcoin()
        {
            // SendBitcoinViewModel sb = new SendBitcoinViewModel();
            //ViewBag.BTC = objCoin.BTCCurrentPrice();//bitcoin 101
            //ViewBag.ETH = objCoin.ETHCurrentPrice();//Etherum Classic 106
            //ViewBag.DASH = objCoin.DASHCurrentPrice();//bitcoinCash 102
            //ViewBag.LTC = objCoin.LTCCurrentPrice();// Bitcoin Gold 103
            //ViewBag.ETC = objCoin.ETCCurrentPrice();// Etherum 105
            //ViewBag.MBC = objCoin.GetMBC_USDCoin();//Micro Bitcoin 104

           // var d = CoinsMarketValueController.GetMarkupWithCoinId();
            foreach (var r in d)
            {
                switch (r.Key)
                {
                    case 101: mark101 = r.Value; break;
                    case 102: mark102 = r.Value; break;
                    case 103: mark103 = r.Value; break;
                    case 104: mark104 = r.Value; break;
                    case 105: mark105 = r.Value; break;
                    case 106: mark106 = r.Value; break;
                }
            }

            try { ViewBag.BTC = "$" + Math.Round((Convert.ToDecimal(objCoin.BTCCurrentPrice()) + Convert.ToDecimal(mark101)), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
            try { ViewBag.ETH = "$" + Math.Round((Convert.ToDecimal(objCoin.ETHCurrentPrice()) + Convert.ToDecimal(mark106)), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
            try { ViewBag.DASH = "$" + Math.Round((Convert.ToDecimal(objCoin.DASHCurrentPrice()) + Convert.ToDecimal(mark102)), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
            try { ViewBag.LTC = "$" + Math.Round((Convert.ToDecimal(objCoin.LTCCurrentPrice()) + Convert.ToDecimal(mark103)), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
            try { ViewBag.ETC = "$" + Math.Round((Convert.ToDecimal(objCoin.ETCCurrentPrice()) + Convert.ToDecimal(mark105)), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
            try { ViewBag.MBC = "$" + Math.Round((Convert.ToDecimal(objCoin.GetMBC_USDCoin()) + Convert.ToDecimal(mark104)), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }


            Notification ob1 = new Notification();
            List<CoinList> coinlist = ob1.GetCoin();
            Session["a"] = coinlist;
            ViewBag.FeeType = GetFeeType();
            ViewBag.Coinlist = coinlist;
            ViewBag.value = 0;
            BlockChainEntities db = new BlockChainEntities();
            string userId = User.Identity.GetUserId();
            var walletbalance = db.GetWalletBalance(userId);
            

            return View();

        }
        public List<SelectListItem> GetFeeType()
        {
            ValueController obj = new ValueController();

            DataSet ds = obj.GetFeeType();
            List<SelectListItem> ls = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                ls.Add(new SelectListItem { Text = dr[0].ToString() });

            }
            Session["FeeType"] = ls;
           return ls;
           
        }
        public ActionResult GetFeeTypeValue(string coinid,string type1)
        {
            ValueController obj = new ValueController();
            decimal? res = obj.GetFeeTypeValue(coinid, type1,SessionToken);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public ActionResult SendBitCoin(SendBitcoinViewModel obj)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Coinlist = Session["a"];
                ViewBag.FeeType = Session["FeeType"];
               // var data = CoinsMarketValueController.GetMarkupWithCoinId();
                foreach (var r in d)
                {
                    switch (r.Key)
                    {
                        case 101: mark101 = r.Value; break;
                        case 102: mark102 = r.Value; break;
                        case 103: mark103 = r.Value; break;
                        case 104: mark104 = r.Value; break;
                        case 105: mark105 = r.Value; break;
                        case 106: mark106 = r.Value; break;
                    }
                }

                try { ViewBag.BTC = "$" + Math.Round((Convert.ToDecimal(objCoin.BTCCurrentPrice()) + Convert.ToDecimal(mark101)), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
                try { ViewBag.ETH = "$" + Math.Round((Convert.ToDecimal(objCoin.ETHCurrentPrice()) + Convert.ToDecimal(mark106)), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
                try { ViewBag.DASH = "$" + Math.Round((Convert.ToDecimal(objCoin.DASHCurrentPrice()) + Convert.ToDecimal(mark102)), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
                try { ViewBag.LTC = "$" + Math.Round((Convert.ToDecimal(objCoin.LTCCurrentPrice()) + Convert.ToDecimal(mark103)), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
                try { ViewBag.ETC = "$" + Math.Round((Convert.ToDecimal(objCoin.ETCCurrentPrice()) + Convert.ToDecimal(mark105)), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
                try { ViewBag.MBC = "$" + Math.Round((Convert.ToDecimal(objCoin.GetMBC_USDCoin()) + Convert.ToDecimal(mark104)), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

                return View(obj);
            }
            string token = SessionToken;
                var userId = User.Identity.GetUserId();
            obj.UserId = userId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/SendPost");

                //HTTP POST
                var postTask = client.PostAsJsonAsync(url+"Transaction/SendPost?sessiontoken="+token,obj);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //return RedirectToAction("Index");
                    EmailNotification en = Session["Notification"] as EmailNotification;
                    if (en.send == true)
                    {
                        string uname = Session["UserName"] as string;
                        string Email = Session["Email"] as string;
                        string res = "Hi," + uname + " you Send coin. ";
                        //SendMail a = new Blockchaninaccounts.App_Start.SendMail();
                        SendMail.Mail(Email, res);
                    }
                    ModelState.Clear();
                    ViewBag.msg = "All Details Saved Successfully";
                    ViewBag.Coinlist = Session["a"];
                    ViewBag.FeeType = Session["FeeType"];

                   // var d = CoinsMarketValueController.GetMarkupWithCoinId();
                    foreach (var r in d)
                    {
                        switch (r.Key)
                        {
                            case 101: mark101 = r.Value; break;
                            case 102: mark102 = r.Value; break;
                            case 103: mark103 = r.Value; break;
                            case 104: mark104 = r.Value; break;
                            case 105: mark105 = r.Value; break;
                            case 106: mark106 = r.Value; break;
                        }
                    }

                    try { ViewBag.BTC = "$" + Math.Round((Convert.ToDecimal(objCoin.BTCCurrentPrice()) + Convert.ToDecimal(mark101)), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
                    try { ViewBag.ETH = "$" + Math.Round((Convert.ToDecimal(objCoin.ETHCurrentPrice()) + Convert.ToDecimal(mark106)), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
                    try { ViewBag.DASH = "$" + Math.Round((Convert.ToDecimal(objCoin.DASHCurrentPrice()) + Convert.ToDecimal(mark102)), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
                    try { ViewBag.LTC = "$" + Math.Round((Convert.ToDecimal(objCoin.LTCCurrentPrice()) + Convert.ToDecimal(mark103)), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
                    try { ViewBag.ETC = "$" + Math.Round((Convert.ToDecimal(objCoin.ETCCurrentPrice()) + Convert.ToDecimal(mark105)), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
                    try { ViewBag.MBC = "$" + Math.Round((Convert.ToDecimal(objCoin.GetMBC_USDCoin()) + Convert.ToDecimal(mark104)), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }
                    ViewBag.value = 1;
                    ModelState.Clear();
                    return View();
                    //return View("DashboardSuccess");


                }
                //else
                //{
                //    ModelState.Clear();
                //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                //}
            }
            ModelState.Clear();
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            ViewBag.FeeType = Session["FeeType"];
            ViewBag.Coinlist = Session["a"];

           // var data = CoinsMarketValueController.GetMarkupWithCoinId();
            foreach (var r in d)
            {
                switch (r.Key)
                {
                    case 101: mark101 = r.Value; break;
                    case 102: mark102 = r.Value; break;
                    case 103: mark103 = r.Value; break;
                    case 104: mark104 = r.Value; break;
                    case 105: mark105 = r.Value; break;
                    case 106: mark106 = r.Value; break;
                }
            }

            try { ViewBag.BTC = "$" + Math.Round((Convert.ToDecimal(objCoin.BTCCurrentPrice()) + Convert.ToDecimal(mark101)), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
            try { ViewBag.ETH = "$" + Math.Round((Convert.ToDecimal(objCoin.ETHCurrentPrice()) + Convert.ToDecimal(mark106)), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
            try { ViewBag.DASH = "$" + Math.Round((Convert.ToDecimal(objCoin.DASHCurrentPrice()) + Convert.ToDecimal(mark102)), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
            try { ViewBag.LTC = "$" + Math.Round((Convert.ToDecimal(objCoin.LTCCurrentPrice()) + Convert.ToDecimal(mark103)), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
            try { ViewBag.ETC = "$" + Math.Round((Convert.ToDecimal(objCoin.ETCCurrentPrice()) + Convert.ToDecimal(mark105)), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
            try { ViewBag.MBC = "$" + Math.Round((Convert.ToDecimal(objCoin.GetMBC_USDCoin()) + Convert.ToDecimal(mark104)), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }


            return View();
        }
       
    }
}