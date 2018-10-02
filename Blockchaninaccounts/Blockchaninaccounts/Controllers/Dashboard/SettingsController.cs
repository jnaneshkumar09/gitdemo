using Blockchaninaccounts.App_Start;
using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blockchaninaccounts.Controllers.Dashboard
{
    [Session]
    [Authorize]
    public class SettingsController : Controller
    {
        decimal? mark101 = 0;
        decimal? mark102 = 0;
        decimal? mark103 = 0;
        decimal? mark104 = 0;
        decimal? mark105 = 0;
        decimal? mark106 = 0;

        HttpClient client;
        APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
        SettingViewMode s = new SettingViewMode();
        //string url = "http://localhost:57986/api/User";
        string url = ConfigurationManager.AppSettings["Url"];
        private ApplicationUserManager _userManager;
        public SettingsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }
        public SettingsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
        // GET: Settings
       
        public ActionResult GetState( int Country)
        {
            ValueController ob1 = new ValueController();
            DataSet ds = ob1.GetState(Country);

            List<State> statelist = new List<State>();

            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                statelist.Add(new State { StateName = dr["StateName"].ToString(), StateId = Convert.ToInt32(dr["StateId"]) });

            }

            return Json(statelist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCity(int State)
        {
            ValueController ob1 = new ValueController();
            DataSet ds = ob1.GetCity(State);

            List<City> citylist = new List<City>();

            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                citylist.Add(new City { CityName = dr["CityName"].ToString(), CityId = Convert.ToInt32(dr["CityId"]) });

            }

            return Json(citylist, JsonRequestBehavior.AllowGet);
        }
         public List<Country> GetCountry()
        {
            ValueController ob1 = new ValueController();
            DataSet ds = ob1.GetCountry();

            List<Country> coutrylist = new List<Country>();

            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                coutrylist.Add(new Country { CountryName = dr["CountryName"].ToString(), CountryId = Convert.ToInt32(dr["CountryId"]) });

            }
            return coutrylist;
        }
      
        public ActionResult Settings()
        {
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

            //SettingViewMode s = new SettingViewMode();
           // var Profile = ValueController.GetCutomerProfile(User.Identity.GetUserId());
           // s.profile = Profile;
            GetCustomerProfile(User.Identity.GetUserId());
            var res = GetCountry();
            Session["Country"] = res;
            ViewBag.Country = res;
            if (s.profile == null) { ViewBag.ProfileData = 0; }
            return View("Settings",s);
 
        }  
        public ActionResult GetCustomerProfile(string userid)
        {
            // CustumerViewMode cv = new CustumerViewMode();
            // CustumerProfileModel res = new CustumerProfileModel();
            try
            {
                var res = ValueController.GetCutomerProfile(userid);
                if (res.Fname!=null && res.Lname!=null && res.Mname!=null)
                {
                    string address = res.FullAddress.Trim();
                    string[] ad = address.Split(',');
                    res.DoorNo = ad[0];
                    res.Address1 = ad[1];
                    res.Address2 = ad[2];
                    res.Zipcode = ad[3];
                    s.profile = res;
                }
                else
                {
                    s.profile = null;
                }
            }
            catch (Exception ex)
            {

                //ViewBag.error = ex;
                if(ex!=null)
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View("Settings", s);
        }
        public new ActionResult Profile(SettingViewMode obj)
        {
            if (!ModelState.IsValid)
            {
                try { ViewBag.BTC = "$" + Math.Round(Convert.ToDecimal(objCoin.BTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
                try { ViewBag.ETH = "$" + Math.Round(Convert.ToDecimal(objCoin.ETHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
                try { ViewBag.DASH = "$" + Math.Round(Convert.ToDecimal(objCoin.DASHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
                try { ViewBag.LTC = "$" + Math.Round(Convert.ToDecimal(objCoin.LTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
                try { ViewBag.ETC = "$" + Math.Round(Convert.ToDecimal(objCoin.ETCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
                try { ViewBag.MBC = "$" + Math.Round(Convert.ToDecimal(objCoin.GetMBC_USDCoin()), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

                ViewBag.Country = Session["Country"];
                return View("Settings",obj);
            }
           var re= ModelState.Values;
            var userId = User.Identity.GetUserId();
            obj.profile.UserId = userId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/Profile");

                //HTTP POST
                var postTask = client.PostAsJsonAsync(url+"Transaction/Profile", obj);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["success"] = "Profile has been updated....!";
                    ModelState.Clear();
                    return RedirectToAction("Settings");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
            }

            //  ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
           
            ViewBag.Country = Session["Country"];

            try { ViewBag.BTC = "$" + Math.Round(Convert.ToDecimal(objCoin.BTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
            try { ViewBag.ETH = "$" + Math.Round(Convert.ToDecimal(objCoin.ETHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
            try { ViewBag.DASH = "$" + Math.Round(Convert.ToDecimal(objCoin.DASHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
            try { ViewBag.LTC = "$" + Math.Round(Convert.ToDecimal(objCoin.LTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
            try { ViewBag.ETC = "$" + Math.Round(Convert.ToDecimal(objCoin.ETCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
            try { ViewBag.MBC = "$" + Math.Round(Convert.ToDecimal(objCoin.GetMBC_USDCoin()), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

            return View("Settings",obj);
        }
        public  ActionResult EmailNotification(SettingViewMode obj)
        {
            if (!ModelState.IsValid)
            {
                try { ViewBag.BTC = "$" + Math.Round(Convert.ToDecimal(objCoin.BTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
                try { ViewBag.ETH = "$" + Math.Round(Convert.ToDecimal(objCoin.ETHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
                try { ViewBag.DASH = "$" + Math.Round(Convert.ToDecimal(objCoin.DASHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
                try { ViewBag.LTC = "$" + Math.Round(Convert.ToDecimal(objCoin.LTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
                try { ViewBag.ETC = "$" + Math.Round(Convert.ToDecimal(objCoin.ETCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
                try { ViewBag.MBC = "$" + Math.Round(Convert.ToDecimal(objCoin.GetMBC_USDCoin()), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

                ViewBag.Country = Session["Country"];
                return View("Settings",obj);
            }

            var userId = User.Identity.GetUserId();
            obj.En.UserId = userId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url+"Transaction/Email");

                //HTTP POST
                var postTask = client.PostAsJsonAsync(url+"Transaction/Email", obj);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    //return RedirectToAction("Index");
                    Notification n = new Notification();

                    EmailNotification en = n.GetNotification(userId);
                    Session["Notification"] = en;
                    //ViewBag.msg = "Data Successfully Saved..!";
                    TempData["successEmail"] = "Notification has been updated....!";
                    //ModelState.Clear();
                    //ViewBag.Country = Session["Country"];
                    //return View("Settings");
                    return RedirectToAction("Settings");


                }
                //else
                //{
                //    ModelState.AddModelError("Email", "Server Error. Please contact administrator.");
                //    //return RedirectToAction("Settings");
                //}
            }
            ModelState.Clear();
              ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            ViewBag.Country = Session["Country"];

            try { ViewBag.BTC = "$" + Math.Round(Convert.ToDecimal(objCoin.BTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
            try { ViewBag.ETH = "$" + Math.Round(Convert.ToDecimal(objCoin.ETHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
            try { ViewBag.DASH = "$" + Math.Round(Convert.ToDecimal(objCoin.DASHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
            try { ViewBag.LTC = "$" + Math.Round(Convert.ToDecimal(objCoin.LTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
            try { ViewBag.ETC = "$" + Math.Round(Convert.ToDecimal(objCoin.ETCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
            try { ViewBag.MBC = "$" + Math.Round(Convert.ToDecimal(objCoin.GetMBC_USDCoin()), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

            return View("Settings");

        }
        public ActionResult SessionTimeout(SettingViewMode obj)
        {
            string time = obj.lg.LogoutTime;
           if(System.Web.HttpContext.Current.Session.Timeout >0 && Session["UserName"]!=null)
            {
              
                switch(time)
                {
                    case "5 Min":
                        Session.Timeout = 5;
                        break;
                    case "10 Min":
                        Session.Timeout = 10;
                        break;
                    case "15 Min":
                        Session.Timeout = 15;
                        break;
                    case "30 Min":
                        Session.Timeout = 30;
                        break;
                    default:
                        Session.Timeout = 20;
                        break;

                }
                ViewBag.res = "You will logout after" + time;
                ViewBag.Country = Session["Country"];
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
                return View("Settings");
            }
            ViewBag.Country = Session["Country"];
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
            return View("Settings");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(SettingViewMode obj)
        {
            if (!ModelState.IsValid)
            {
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

                ViewBag.Country = Session["Country"];
                return View("Settings", obj);
            }
            var userId = User.Identity.GetUserId();
            obj.cp.UserId = userId;
            using (client)
            {


                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "User/" + "ChangePassword", obj);

                if (responseMessage.IsSuccessStatusCode)
                {
                    SettingViewMode obj1 = new SettingViewMode();
                   
                    obj1.cp = new ChangePasswordViewModel();

                    ViewBag.Country = Session["Country"];
                    ViewBag.StatusMessage = "Your password has been changed.";

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

                    return View("Settings", obj1);
                   
                }
                else
                {
                    string msg = UserController.getmsg;
                    if (msg!=null)
                    {
                        ModelState.AddModelError("ch", msg);
                    }
                  //  ModelState.AddModelError("Change", "Server Error. Please contact administrator.");

                }
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
                ViewBag.Country = Session["Country"];
                return View("Settings", obj);
            }
        }
        public async Task<JsonResult> GetSecretPhase()

        { 
            var userId = User.Identity.GetUserId();
            var user1 = await UserManager.FindByIdAsync(userId);
            string res = user1.SecretPhase;
            return Json(res, JsonRequestBehavior.AllowGet);

        }
    }
}