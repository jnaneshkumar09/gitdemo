using Blockchaninaccounts.App_Start;
using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
    public class OrderHistoryController : Controller
    {
        decimal? mark101 = 0;
        decimal? mark102 = 0;
        decimal? mark103 = 0;
        decimal? mark104 = 0;
        decimal? mark105 = 0;
        decimal? mark106 = 0;

        APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
        string url = ConfigurationManager.AppSettings["Url"];
        // GET: OredrHistory
        public ActionResult Index()
        {

            return null;
        }
        [HttpGet]
        public ActionResult OrderHistory()
        {
            //var id = User.Identity.GetUserId();
            ////IEnumerable<AllHistory> students = null;

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://localhost:57986/api/Transaction/Getid");
            //    //HTTP GET
            //    var responseTask = client.GetAsync("http://localhost:57986/api/Transaction/Getid?id=" + id);
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<IList<AllHistory>>();
            //        readTask.Wait();

            //        ViewBag.AllResult = readTask.Result;

            //    }
            //    else //web api sent error response 
            //    {
            //        //log response status here..
            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
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
            GetSend();
            GetReceive();
            GetBuy();
            GetSell();
            GetExchange();
            return View();
        }
        public ActionResult GetSend()
        {
            var userid = User.Identity.GetUserId();
            //IEnumerable<AllHistory> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/GetSend");
                //HTTP GET
                var responseTask = client.GetAsync(url+"Transaction/GetSend?userid=" + userid);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<SendBitcoinViewModel>>();
                    readTask.Wait();

                    ViewBag.AllResult = readTask.Result;
                    return View("OrderHistory");

                }
                //else //web api sent error response 
                //{
                //    //log response status here..
                //    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                //}
            }
            // ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            ViewBag.errorSend = "you don't have send request..";
            return View("OrderHistory");
        }
        public ActionResult GetReceive()
        {
            var userid = User.Identity.GetUserId();
            //IEnumerable<AllHistory> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/GetRec");
                //HTTP GET
                var responseTask = client.GetAsync(url+"Transaction/GetRec?userid=" + userid);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<RecieveBitcoinViewModel>>();
                    readTask.Wait();

                    ViewBag.ReceiveResult = readTask.Result;
                    return View("OrderHistory");
                }
                //else //web api sent error response 
                //{
                //    //log response status here..
                //    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                //}
            }
            ViewBag.errorRec = "you don't have Receive request..";
            return View("OrderHistory");
        }
        public ActionResult GetBuy()
        {
            var userid = User.Identity.GetUserId();
            //IEnumerable<AllHistory> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/GetBuy");
                //HTTP GET
                var responseTask = client.GetAsync(url+"Transaction/GetBuy?userid=" + userid);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<BuyViewModel>>();
                    readTask.Wait();

                    ViewBag.BuyResult = readTask.Result;
                    return View("OrderHistory");
                }
                //else //web api sent error response 
                //{
                //    //log response status here..
                //    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                //    ViewBag.BuyResult = null;
                //}
            }
            ViewBag.errorBuy = "you don't have Buy request..";
            return View("OrderHistory");
        }
        public ActionResult GetSell()
        {
            var userid = User.Identity.GetUserId();
            //IEnumerable<AllHistory> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/GetSell");
                //HTTP GET
                var responseTask = client.GetAsync(url+"Transaction/GetSell?userid=" + userid);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<SellViewModel>>();
                    readTask.Wait();

                    ViewBag.SellResult = readTask.Result;
                    return View("OrderHistory");
                }
                //else //web api sent error response 
                //{
                //    log response status here..
                //    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                //    ViewBag.SellResult = null;
                //}
            }
            ViewBag.errorSell = "you don't have Sell request..";
            return View("OrderHistory");
        }
        public ActionResult GetExchange()
        {
            var userid = User.Identity.GetUserId();
            //IEnumerable<AllHistory> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/GetEx");
                //HTTP GET
                var responseTask = client.GetAsync(url+"Transaction/GetEx?userid=" + userid);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ExchangeViewMode>>();
                    readTask.Wait();

                    ViewBag.ExResult = readTask.Result;
                    return View("OrderHistory");
                }
                //else //web api sent error response 
                //{
                //    //log response status here..
                //    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                //    ViewBag.ExResult = null;
                //}
            }
            ViewBag.errorEx = "you don't have Exchange request..";
            return View("OrderHistory");
        }
    }
}