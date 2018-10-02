using Blockchaninaccounts.App_Start;
using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;


namespace Blockchaninaccounts.Controllers.Dashboard
{
    [Session]
    [Authorize]
    public class RecieveController : Controller
    {
       public static Dictionary<int, decimal?> d = CoinsMarketValueController.GetMarkupWithCoinId();
        decimal? mark101 = 0;

        decimal? mark102 = 0;
        decimal? mark103 = 0;
        decimal? mark104 = 0;
        decimal? mark105 = 0;
        decimal? mark106 = 0;
        
     

       APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
        string url = ConfigurationManager.AppSettings["Url"];
        // GET: Recieve
        public ActionResult Index()
        {
            return null;
        }
        public ActionResult RecieveBitcoin()
        {
            
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
       // [ActionName("PostSaveReceive")]
        public ActionResult RecieveBitcoin(RecieveBitcoinViewModel obj)
        {
           // string CoinName = Request.Form["CoinTypeId"];
            string CoinName = Request["CoinName"];
            obj.QRCode = Request["Address"];
            obj.DestinationBitcoin = "Sample";
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
                return View(obj);
            }

            var userId = User.Identity.GetUserId();
            obj.UserId = userId;
           // using (var scope = new TransactionScope())
            { 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"transaction/ReceiveaPost");
                    

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync(url+"transaction/ReceiveaPost", obj);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                        string message = "Hi, this " +Session["Email"] as string+" wants "+obj.AmtInCoins+" " +CoinName+" and Coin Address is "+ Request["Address"];
                        SendMail.Mail(obj.RecievedToEmail, message);
                        //return RedirectToAction("Index");
                        EmailNotification en = Session["Notification"] as EmailNotification;
                        if (en != null)
                        {
                            if (en.Receive == true)
                            {
                                string uname = Session["UserName"] as string;
                                string Email = Session["Email"] as string;
                                string res = "Hi," + uname + " you sent Recieve coin requist. ";
                                //SendMail a = new Blockchaninaccounts.App_Start.SendMail();
                                SendMail.Mail(Email, res);
                            }
                        }
                        ViewBag.msg = "Receive Request successful...!";

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
        }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
           // ModelState.Clear();
            ViewBag.Coinlist = Session["a"] ;

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