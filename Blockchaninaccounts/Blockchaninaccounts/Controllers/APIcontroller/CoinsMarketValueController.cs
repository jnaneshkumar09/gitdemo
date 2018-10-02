using BittrexApi;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Blockchaninaccounts.Controllers.APIcontroller
{
    public class CoinsMarketValueController : Controller
    {
        //CoinsMarketValueController obj = new CoinsMarketValueController();
     
        //dynamic obj = GetMarkupWithCoinId();
        //APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
        public string bittrexapi_key = ConfigurationSettings.AppSettings["BittrexApikey"];
        public string bittrexsecert_key = ConfigurationSettings.AppSettings["BittrexSecertKey"];
        public string bittrexbase_address = ConfigurationSettings.AppSettings["BittrexBaseAddress"];
        public string bittrexapi_version = ConfigurationSettings.AppSettings["BittrexApiVersion"];

        // GET: CoinsMarketValue
        public ActionResult Index()
        {
          // var x= obj.GetMarkupWithCoinId();
            
            return View();
        }

        // public string GetMBC_USDCoin()//Micro Bitcoin 104
        //public string GetMBC_USDCoin()//Micro Bitcoin 104
        //{
        //    try
        //    {
        //        string link = "http://api.huobi.pro/market/depth?symbol=xrpusdt&type=step1";// https://api.huobi.pro/market/depth?symbol=xrpusdt&type=step1
        //             //string link = "http://aristrexfx.com/belfrics/api/v1/marketprice?marketName=MBCBTC,MBCUSD";

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);

        //        request.MaximumAutomaticRedirections = 4;
        //        request.MaximumResponseHeadersLength = 4;

        //        request.Credentials = CredentialCache.DefaultCredentials;
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        try { }
        //        catch (Exception es) { }
        //        Stream receiveStream = response.GetResponseStream();
        //        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
        //        string responseFromServer = readStream.ReadToEnd();
        //        char chrQuote = (char)34;
        //        responseFromServer = responseFromServer.Replace(chrQuote + "data" + chrQuote + ":", "").Replace("," + chrQuote + "isSuccess" + chrQuote + ":true", "");
        //        // string mbcvalue = "$" + responseFromServer.Substring(34, 10);
        //        string mbcvalue = responseFromServer.Substring(34, 10);
        //        return mbcvalue;


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public string BTCCurrentPrice()//bitcoin 101
          {

              try
              {
                  BittrexClient bitt = new BittrexClient(bittrexbase_address, bittrexapi_key, bittrexsecert_key);
        var a = bitt.Getticker("USDT-BTC");//USDT-LTC
        dynamic d = JObject.Parse(a);
        var a1 = d.result.Bid;
        var last = d.result.Last;
                  return last;
              }
              catch (Exception es)
              {
                  throw es;
              }
          }
        public string GetMBC_USDCoin()//Litecoin 104
        {

            try
            {
                BittrexClient bitt = new BittrexClient(bittrexbase_address, bittrexapi_key, bittrexsecert_key);
                var a = bitt.Getticker("USDT-LTC");//USDT-LTC
                dynamic d = JObject.Parse(a);
                var a1 = d.result.Bid;
                var last = d.result.Last;
                return last;
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        public string ETHCurrentPrice()//Etherum Classic 106
        {
            
            try
            {
                BittrexClient bitt = new BittrexClient(bittrexbase_address, bittrexapi_key, bittrexsecert_key);
                var b = bitt.Getticker("USDT-ETH");
                dynamic d1 = JObject.Parse(b);
                var b1 = d1.result.Bid;
                var last = d1.result.Last;
                return last;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DASHCurrentPrice()//bitcoinCash 102
        {
            
            try
            {
                BittrexClient bitt = new BittrexClient(bittrexbase_address, bittrexapi_key, bittrexsecert_key);
                var c = bitt.Getticker("USDT-DASH");
                dynamic d2 = JObject.Parse(c);
                var c1 = d2.result.Bid;
                var dash = d2.result.Last;
                return dash;
            }   
            catch (Exception ex)
            {
                  throw ex;
            }
        }

        public string LTCCurrentPrice()// Bitcoin Gold 103
        {
            
            try
            {
                BittrexClient bitt = new BittrexClient(bittrexbase_address, bittrexapi_key, bittrexsecert_key);
                var c = bitt.Getticker("USDT-LTC");
                dynamic d3 = JObject.Parse(c);
                var c2 = d3.result.Bid;
                var ltc = d3.result.Last;
                return ltc;
            }
            catch (Exception ex)  
            {
                throw ex;
            }
        }

        public string ETCCurrentPrice()// Etherum 105
        {
           
            try
            {
                BittrexClient bitt = new BittrexClient(bittrexbase_address, bittrexapi_key, bittrexsecert_key);
                var c = bitt.Getticker("USDT-ETC");
                dynamic d4 = JObject.Parse(c);
                var c3 = d4.result.Bid;
                var etc = d4.result.Last;
                return etc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetCoinValue(int coinid, int value, string coinname=null, string name = null, int Recoinid = 0)
        {
            decimal? markupvalue = 0;
            decimal? Rmarkupvalue = 0;
            decimal? Total = 0;
            string[] FeeTax = null;
            decimal Fee = 0;
            decimal Tax = 0;
            decimal Tax1 = 0;
            decimal? coinBal;
            string error = "";
            decimal CoinTax = 0;
            decimal CoinFee = 0;

            if (name != null)
            {
                switch (name)
                {
                    case "Buy": markupvalue = GetMarkupBuy(coinid); FeeTax = GetBuyFeeTax(coinid); if (Recoinid != 0) { Rmarkupvalue = GetMarkupBuy(Recoinid); } break;
                    case "Sell": markupvalue = GetMarkupSell(coinid); FeeTax = GetSellFeeTax(coinid); if (Recoinid != 0) { Rmarkupvalue = GetMarkupSell(Recoinid); } break;
                    case "Exchange": markupvalue = GetMarkupExchange(coinid); if (Recoinid != 0) { Rmarkupvalue = GetMarkupExchange(Recoinid); } break;
                    default:break;
                }
            }
            if(FeeTax!=null)
            {
                 CoinFee = Convert.ToDecimal(FeeTax[0]);
                 CoinTax = Convert.ToDecimal(FeeTax[1]);
                Fee = CoinFee * value;
                 Tax = CoinTax * value;
            }
            
           
            // APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
            //string ReceiveCoinValue;
            string coinvalue = GetCoinValue1(coinid);

            if (coinvalue.Contains('$')) { coinvalue = coinvalue.Replace("$", ""); }
            if (coinvalue == "NA") { return Json(coinvalue, JsonRequestBehavior.AllowGet); }
            decimal? coinamt = (Convert.ToDecimal(coinvalue) + markupvalue) * value;

            if (name == "Send"|| name=="Exchange")
            {
                coinBal = GetCoinBal(coinname);
                if (coinBal >= value) { error = "Yes"; }
                else
                {
                    decimal? ReceiveAmt = 0, Baldiff = 0;
                    //if (name == "Sell") { Total = coinamt + Fee + (Fee * (Tax / 100)); }
                    if (Recoinid != 0) { string ReceiveCoinValue = GetCoinValue1(Recoinid); ReceiveAmt = coinamt / (Convert.ToDecimal(ReceiveCoinValue) + Rmarkupvalue); }
                    Baldiff = ReceiveAmt - coinBal;
                     error = "NOBAL";
                    object obj1 = new { coinamt, error, Baldiff, ReceiveAmt };
                    return Json(obj1, JsonRequestBehavior.AllowGet);
                }
            }

            if (Recoinid != 0)
            {
               
                string ReceiveCoinValue= GetCoinValue1(Recoinid);
                if (ReceiveCoinValue == "NA") { return Json(ReceiveCoinValue, JsonRequestBehavior.AllowGet); }
                decimal? ReceiveAmt = coinamt / (Convert.ToDecimal(ReceiveCoinValue) + Rmarkupvalue);
                object obj1 = new { markupvalue, Rmarkupvalue, ReceiveAmt };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }
            if (name == "Buy" || name == "Sell")
            {
                Tax1 = (Fee * (Tax / 100));
                Total = coinamt + Fee + (Fee * (Tax/100));
                if (name == "Sell")
                {
                    coinBal = GetCoinBal(coinname);
                    if(coinBal>= value)
                    {
                        
                    }
                    else
                    {
                         error = "NOBAL";
                        object obj1 = new { Total, error, Fee, Tax1, coinamt, markupvalue };
                        return Json(obj1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
           
            object obj = new { markupvalue, coinamt, Total,Fee, CoinTax, CoinFee, Tax1,error };//for buy,Sell,Send,Receive
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public decimal? GetCoinBal(string coinname)
        {
            BlockChainEntities db = new BlockChainEntities();
            string userId = User.Identity.GetUserId();
            var walletbalance = db.GetWalletBalance(userId);
           
            decimal? i= 0;
            foreach (var a in walletbalance)
            {
                if(a.CoinName== coinname)
                {
                    i = a.AvailableBalance;
                    break;
                }
            }
            return i;
        }
        public static decimal? GetMarkupBuy(int coinid)
        {
            BlockChainEntities ob = new BlockChainEntities();
           //var res = ob.tbl_Coin_Master;
            var markupvalue = (from x in ob.tbl_Coin_Master where x.Id == coinid select x.MarkupBuy).FirstOrDefault();
            // var aaaa = (ob.tbl_Coin_Master.Where(x => x.Id == coinid).Select(x => new { x.BuyFee, x.BuyTax })).ToList();
             return markupvalue;
        }//FirstOrDefault();
        public decimal? GetMarkupSell(int coinid)
        {
            BlockChainEntities ob = new BlockChainEntities();
            //var res = ob.tbl_Coin_Master;
            var markupvalue = (from x in ob.tbl_Coin_Master where x.Id == coinid select x.MarkupSell).FirstOrDefault();
            return markupvalue;
        }
        public decimal? GetMarkupExchange(int coinid)
        {
            BlockChainEntities ob = new BlockChainEntities();
            //var res = ob.tbl_Coin_Master;
            var markupvalue = (from x in ob.tbl_Coin_Master where x.Id == coinid select x.MarkupExchange).FirstOrDefault();
            return markupvalue;
        }
        public string[] GetBuyFeeTax(int coinid)
        {
            string[] a = new string[2];
            BlockChainEntities ob = new BlockChainEntities();
            var e= (ob.tbl_Coin_Master.Where(x => x.Id == coinid).Select(x => new { x.BuyFee, x.BuyTax })).FirstOrDefault();
            a[0]= e.BuyFee.ToString();
            a[1] = e.BuyTax.ToString();
            return a;
        }
        public string[] GetSellFeeTax(int coinid)
        {
            string[] a = new string[2];
            //string re=a[0]
            BlockChainEntities ob = new BlockChainEntities();
            var e = (ob.tbl_Coin_Master.Where(x => x.Id == coinid).Select(x => new { x.SellFee, x.SellTax })).FirstOrDefault();
           // var res = (ob.tbl_Coin_Master.Select(x => new { x.Id, x.MarkupBuy })).FirstOrDefault();
            //a[0] = Convert.ToString(e.SellFee); a[1] = Convert.ToString(e.SellTax);
            decimal? a1 = e.SellFee; decimal? a2 = e.SellTax;
            string s1 = a1.ToString(); string s2 = a2.ToString();
            a[0] = s1; a[1] = s2;
            return a;
         
        }
        public static Dictionary<int,decimal?> GetMarkupWithCoinId()
        {
            Dictionary<int, decimal?> d = new Dictionary<int, decimal?>();
            BlockChainEntities ob = new BlockChainEntities();
            var data = (ob.tbl_Coin_Master.Select(x => new { x.Id, x.MarkupBuy })).ToList();
            foreach(var r in data)
            {
                d.Add(r.Id, r.MarkupBuy);
            }
            return d;
        }
        
        public ActionResult CheckCoin(int coinid,string coinname=null)//-----------------bal
        {
           
            string coinvalue = GetCoinValue1(coinid);
            if (coinvalue == "NA") { return Json(coinvalue, JsonRequestBehavior.AllowGet); }
            else
            {
                if(coinname!=null)
                {
                    coinvalue = ValueController.GetAddress(coinname, User.Identity.GetUserId());
                    if (coinvalue != null)
                    {
                        string Qr = Qrcode(coinvalue);
                        object data = new { coinvalue,Qr };
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                         return Json("Null", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(coinvalue, JsonRequestBehavior.AllowGet);
            }
          
        }
        public string Qrcode(string address)
        {
            string qrcodeimage = "";
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    qrcodeimage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }

            }
            return qrcodeimage;
        }

        public string GetCoinValue1(int coinid)
        {
            APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
            string coinvalue;
            switch (coinid)
            {
                case 101: try { coinvalue = objCoin.BTCCurrentPrice(); } catch (Exception ex) { coinvalue = "NA"; } break;//bitcoin 101
                case 102: try { coinvalue = objCoin.DASHCurrentPrice(); } catch (Exception ex) { coinvalue = "NA"; } break;//bitcoinCash 102
                case 103: try { coinvalue = objCoin.LTCCurrentPrice(); } catch (Exception ex) { coinvalue = "NA"; } break;// Bitcoin Gold 103
                case 104: try { coinvalue = objCoin.GetMBC_USDCoin(); } catch (Exception ex) { coinvalue = "NA"; } break;//Micro Bitcoin 104
                case 105: try { coinvalue = objCoin.ETCCurrentPrice(); } catch (Exception ex) { coinvalue = "NA"; } break;// Etherum 105
                case 106: try { coinvalue = objCoin.ETHCurrentPrice(); } catch (Exception ex) { coinvalue = "NA"; } break;//Etherum Classic 106
                default: return null;
            }
            if (coinvalue == "NA") { return coinvalue; }
            else { return coinvalue; }
        }
    }
}