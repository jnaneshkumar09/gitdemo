using Blockchaninaccounts.App_Start;
using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Blockchaninaccounts.Controllers.Dashboard
{
    [Session]
    [Authorize]
    public class SellBitcoinController : Controller
    {
        decimal? mark101 = 0;
        decimal? mark102 = 0;
        decimal? mark103 = 0;
        decimal? mark104 = 0;
        decimal? mark105 = 0;
        decimal? mark106 = 0;
       // var d = CoinsMarketValueController.GetMarkupWithCoinId();
        APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
        string url = ConfigurationManager.AppSettings["Url"];
        private ApplicationUserManager _userManager;
        public SellBitcoinController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }
        public SellBitcoinController()
        {

        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: SellBitcon
        public ActionResult Index()
        {
            return null;
        }
        public ActionResult SellBitcoin()
        {
            //SellViewModel obj = new SellViewModel();
            //ValueController ob1 = new ValueController();
            //obj.CoinList = ob1.GetCoin().ToList();
            //Session["a"] = obj.CoinList;
            //return View(obj);
            
            //decimal? markup = 0;
            var d = CoinsMarketValueController.GetMarkupWithCoinId();
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
            ViewBag.Coinlist = coinlist;
            return View();
        }
    
        [HttpPost]
        public ActionResult SellBitcoin(SellViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                var d1 = CoinsMarketValueController.GetMarkupWithCoinId();
                foreach (var r in d1)
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

                ViewBag.Coinlist = Session["a"];
                return View(obj);
            }

            var userId = User.Identity.GetUserId();
            var user = UserManager.FindByIdAsync(userId).Result;
            obj.UserId = userId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/Sell");

                //HTTP POST
                var postTask = client.PostAsJsonAsync(url+"Transaction/Sell", obj);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //return RedirectToAction("Index");
                    EmailNotification en = Session["Notification"] as EmailNotification;
                    if (en != null)
                    {
                        if (en.Sell == true)
                        {
                            string res = "Hi," + user.UserName + " you Sell coin. ";
                            //SendMail a = new Blockchaninaccounts.App_Start.SendMail();
                            SendMail.Mail(user.Email, res);
                        }
                    }
                    ViewBag.msg = "Sell Amount Saved Successfully";
                    var d2 = CoinsMarketValueController.GetMarkupWithCoinId();
                    foreach (var r in d2)
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
                    ModelState.Clear();
                    return View("DashboardSuccess");


                }
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                //}
            }

             ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            //ModelState.Clear();
            ViewBag.Coinlist = Session["a"];

            var d = CoinsMarketValueController.GetMarkupWithCoinId();
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
    }
}