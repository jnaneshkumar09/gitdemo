using Blockchaninaccounts.App_Start;
using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blockchaninaccounts.Controllers.Dashboard
{
    [Session]
    [Authorize]
    public class UserWalletBalanceController : Controller
    {
        // GET: UserWalletBalance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WalletBalance()
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

            APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();

            try { ViewBag.BTC = "$" + Math.Round((Convert.ToDecimal(objCoin.BTCCurrentPrice()) + Convert.ToDecimal(mark101)), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
            try { ViewBag.ETH = "$" + Math.Round((Convert.ToDecimal(objCoin.ETHCurrentPrice()) + Convert.ToDecimal(mark106)), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
            try { ViewBag.DASH = "$" + Math.Round((Convert.ToDecimal(objCoin.DASHCurrentPrice()) + Convert.ToDecimal(mark102)), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
            try { ViewBag.LTC = "$" + Math.Round((Convert.ToDecimal(objCoin.LTCCurrentPrice()) + Convert.ToDecimal(mark103)), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
            try { ViewBag.ETC = "$" + Math.Round((Convert.ToDecimal(objCoin.ETCCurrentPrice()) + Convert.ToDecimal(mark105)), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
            try { ViewBag.MBC = "$" + Math.Round((Convert.ToDecimal(objCoin.GetMBC_USDCoin()) + Convert.ToDecimal(mark104)), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

            BlockChainEntities db = new BlockChainEntities();
            string userId= User.Identity.GetUserId();
            var walletbalance = db.GetWalletBalance(userId);
            ViewBag.walletbalance = walletbalance;
            return View();
        }
    }
}