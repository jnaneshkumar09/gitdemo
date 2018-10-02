using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Models;
using Google.Authenticator;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Blockchaninaccounts.App_Start;

namespace Blockchaninaccounts.Controllers.Dashboard
{
    [Session]
    [Authorize]
    public class SecurityCenterController :BaseController
    {
        decimal? mark101 = 0;
        decimal? mark102 = 0;
        decimal? mark103 = 0;
        decimal? mark104 = 0;
        decimal? mark105 = 0;
        decimal? mark106 = 0;
        SecurityViewModel SVM = new SecurityViewModel();
        APIcontroller.CoinsMarketValueController objCoin = new APIcontroller.CoinsMarketValueController();
        HttpClient client;
        //string url = "http://localhost:57986/api/User";
        string url = ConfigurationManager.AppSettings["Url"];
        public static string Email  { get; set; }
        public static string Id { set; get; }
        public SecurityCenterController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        ValueController obj = new ValueController();
        private ApplicationUserManager _userManager;
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
        //public string GetKey()
        //{
        //    Random random = new Random();
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@)(*#";
        //    return new string(Enumerable.Repeat(chars, 13)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
        public ActionResult Index()
        {
            return View();
        }
        public void GetQR()
        {
            string key = "xyz";
            bool status = true;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string UserUniqueKey = Email + key; //as Its a demo, I have done this way. But you should use any encrypted value here which will be unique value per user 
            Session["xyz"] = UserUniqueKey;
            var setupInfo = tfa.GenerateSetupCode("Dotnet Awesome", Email, UserUniqueKey, 300, 300);
            ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            ViewBag.SetupCode = setupInfo.ManualEntryKey;
        }
        public async System.Threading.Tasks.Task<ActionResult> Security()
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
            GetQR();
           
            sample s = new sample { TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),GoogleStatus= obj.GetGoogleStatus(User.Identity.GetUserId()) };
            var sm = ValueController.GetSession(User.Identity.GetUserId());
            ViewBag.IpWhiteList = sm.IPAdressWhiteList;
            SVM.s = s; SVM.Sm = sm;
           
            Session["EmailStatus"] = s;
            return View(SVM);
        }
        public ActionResult EnableGoogleTo1()
        {
            var EmailStatus = Session["EmailStatus"] as sample;
            if(EmailStatus.TwoFactor==true)
            {
                ModelState.AddModelError(string.Empty, "Sorry! Your 2FA email is enable, first disable 2FA email authentication.");
                return RedirectToAction("Security");
            }
            return RedirectToAction("EnableGoogle");
        }
        [AllowAnonymous]
        public async System.Threading.Tasks.Task<ActionResult> EnableGoogle()//need Google Enable------1
        {
            var token = Request["passcode"];
            bool isValid = Verify2FA(token);
            var userid = User.Identity.GetUserId();
            if (isValid)
            {
                var callbackUrl = Url.Action("ChangeGoogleStatus", "SecurityCenter", new { userId = userid, value = true }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(userid, "Enable Google Two-factor Authentication",
                    "Please enable your google authentication by clicking <a href=\"" + callbackUrl + "\">here</a>");

                ViewBag.Message = "The two-factor Google enable Link has been sent to your register email id, Please check your email and click on link.";
                return View("Info");
            }
            TempData["GoogleError"] = "Your code is not valid! Please click on 'Enable' and enter valid code.";
            return RedirectToAction("Security");
        }
        [AllowAnonymous]
        public ActionResult ChangeGoogleStatus(string userId, bool value)//need  Google Enable ------2
        {
            // var userid = User.Identity.GetUserId();

            if (value == true)
            {
                //bool value = true;
                try
                {
                    bool res = obj.ChangeGoogleStatus(userId, value);
                    if (res == true)
                    {
                        //Write code for insert activity log in db
                        sample EmailStatus = Session["EmailStatus"] as sample;
                        if (EmailStatus.TwoFactor == true)
                        {
                            bool result = obj.ChangeEmailStatus(userId, false);
                            //EmailFalse(userId, false);
                            //await ChangeEmailStatus("", "");
                        }
                        LogOut();
                        ViewBag.Message = "Your Google Authentication enable...Please";
                        return View("GoogleInfo");
                    }
                }
                catch (Exception ex)
                {
                    TempData["SendAction"] = "Internal Servar Error.";
                }

            }
            else if (value == false)
            {
                try
                {
                    //bool value = false;
                    bool res = obj.ChangeGoogleStatus(userId, value);
                    if (res == true)
                    {

                        //ViewBag.Message = "Your Google Authentication Disable...Please";
                        // return View("Info");
                        return RedirectToAction("Security", "SecurityCenter");
                    }
                }
                catch (Exception ex)
                {
                    TempData["SendAction"] = "Internal Servar Error.";
                }
            }
            return null;
        }
        public ActionResult DisableGoogle()//need--google---3
        {

            var userid = User.Identity.GetUserId();
            return RedirectToAction("ChangeGoogleStatus", "SecurityCenter", new { userId = userid, value = false });
        }

        // [AllowAnonymous]
        //public async Task<ActionResult> ChangeGoogleStatus(string userId, bool value)
        //{
        //    if (userId == null)
        //    {
        //        return View("Error");
        //    }
        //    EmailModel em = new EmailModel{ UserId = userId, value = value };
        //    using (client)
        //    {
        //         HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "/" + "PostEmailEnbble", em);

        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            ViewBag.Message = "Your Google Authentication enable...Please";
        //            LogOut();
        //            return View("GoogleInfo");
        //            //return View("Settings", obj1);

        //        }
        //        else
        //        {
        //            ViewBag.errorMessage = "Server Error. Please contact administrator.";
        //            return View("Error");
        //            // ModelState.AddModelError("Change", "Server Error. Please contact administrator.");

        //        }

        //       // return View("Settings", obj);
        //    }
        //}
        public void LogOut()//need----
        {
            if (User.Identity.IsAuthenticated)
            {
                // Request.IsAuthenticated = false;
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }
        public bool Verify2FA(string gtoken)
        {
            GetQR();
            //var token = Request["passcode"];
            var token = gtoken;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string UserUniqueKey = Session["xyz"].ToString();
            bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
            if (isValid)
            {
               // var res = obj.PostSessionToken();
                Session["IsValid2FA"] = true;
                return true;
               // return RedirectToAction("Index", "Dashboard");
            }
            return false;
            //return RedirectToAction("Login", "Account");
        }
        public ActionResult Verify2faGoogle()
       {
            var token = Request["passcode"];
            bool res = Verify2FA(token);
            if(res)
            {
                string Ip = AccountController.GetIPAddress();
                string Mac = AccountController.GetMacAddress();
                string location = null;
                string Email = SecurityCenterController.Email;
                // var userId = User.Identity.GetUserId();
               // var userId = SecurityCenterController.Id;
               // bool login = obj.PostActivityLog(userId, "Login", "Succeed");
                //write a code for activity log in db
                //var res1 = obj.PostSessionToken();
                //return RedirectToLocal(returnUrl, user.Id, Ip, Mac, location, user.Email);
                return RedirectToAction("RedirectToLocal1", "Account",new {Ip, Mac, location,Email });
                //return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("Login", "Account");
        }
        //public ActionResult Verify2FA()
        //{
        //    GetQR();
        //      var token = Request["passcode"];
        //    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        //    string UserUniqueKey = Session["xyz"].ToString();
        //    bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
        //    if (isValid)
        //    {
        //        var res = obj.PostSessionToken();
        //        Session["IsValid2FA"] = true;
        //        return RedirectToAction("Index", "Dashboard");
        //    }
        //    return RedirectToAction("Login", "Account");
        //}
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableEmail()//need email--1
        {
           
            var userid = User.Identity.GetUserId();
           
                var callbackUrl = Url.Action("ChangeEmailStatus", "SecurityCenter", new { userId = userid, value = true }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(userid, "Enable Email Two-factor Authentication",
                    "Please enable your Email authentication by clicking <a href=\"" + callbackUrl + "\">here</a>");

                ViewBag.Message = "The two-factor Email enable Link has been sent to your register email id, Please check your email and click on link.";
            //LogOut();
            return View("Info");
            
           // TempData["GoogleError"] = "Your code is not valid! Please click on 'Enable' and enter valid code.";
           // return RedirectToAction("Security");
        }
        public ActionResult DisableEmail()//-----email ----3
        {
            var userId = User.Identity.GetUserId();

            return RedirectToAction("ChangeEmailStatus", "SecurityCenter", new { userId = userId, value = false });

        }

        [AllowAnonymous]
        public async System.Threading.Tasks.Task<ActionResult> ChangeEmailStatus(string userId, bool value)//need----email---2
        {
            if (userId == null)
            {
                return View("Error");
            }
            EmailModel em = new EmailModel { UserId = userId, value = value };
            using (client)
            {
                if (value == true)
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "User/" + "PostEmailStatus", em);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Your Email Authentication enable...Please";
                        LogOut();
                        return View("GoogleInfo");
                        //return View("Settings", obj1);

                    }
                    else
                    {
                        ViewBag.errorMessage = "Server Error. Please contact administrator.";
                        return View("Error");
                        // ModelState.AddModelError("Change", "Server Error. Please contact administrator.");

                    }
                }
                else
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url + "User/" + "PostEmailStatus", em);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        // ViewBag.Message = "Your Email Authentication enable...Please";
                        // LogOut();
                        // return View("GoogleInfo");
                        //return View("Settings", obj1);
                        return RedirectToAction("Security", "SecurityCenter");

                    }
                    else
                    {
                        ViewBag.errorMessage = "Server Error. Please contact administrator.";
                        return View("Error");
                        // ModelState.AddModelError("Change", "Server Error. Please contact administrator.");

                    }
                }


                // return View("Settings", obj);
            }
        }
        

       
        //public async Task<ActionResult> EmailFalse(string userId, bool value)
        //{


        //    return  RedirectToAction("ChangeEmailStatus", new { userid = userId, value = value });
        //}

        //public PartialViewResult Details()
        //{
        //    //sample s = new sample();
        //    sample s = new sample();
        //    s.no = 1;
        //    ViewBag.a="Naresh......";
        //    return PartialView("At");
        //}
        //public ActionResult Demo()
        //{
        //    sample s = new sample();
        //    s.no = 1;
        //    Details();
        //    return View(s);
        //}
        public ActionResult PostSession(SecurityViewModel obj,string [] DynamicTextBox)
        {
            if(DynamicTextBox!=null)
            { 
                string id = User.Identity.GetUserId();string Mac = "Null"; string location= "Null";
                foreach( var r in DynamicTextBox)
                {
                    string Ip = r;
                    if(Ip!="")
                    ValueController.PostSecurityData(id, Ip, Mac, location,true);
                }
                TempData["success"] = "IP Adress successfully updated....!";
                return RedirectToAction("Security");
            }
            if (!ModelState.IsValid)
            {
                try { ViewBag.BTC = "$" + Math.Round(Convert.ToDecimal(objCoin.BTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
                try { ViewBag.ETH = "$" + Math.Round(Convert.ToDecimal(objCoin.ETHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
                try { ViewBag.DASH = "$" + Math.Round(Convert.ToDecimal(objCoin.DASHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
                try { ViewBag.LTC = "$" + Math.Round(Convert.ToDecimal(objCoin.LTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
                try { ViewBag.ETC = "$" + Math.Round(Convert.ToDecimal(objCoin.ETCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
                try { ViewBag.MBC = "$" + Math.Round(Convert.ToDecimal(objCoin.GetMBC_USDCoin()), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }
                obj.s = Session["EmailStatus"] as sample;
                 return View("Settings",obj);
            }

            var userId = User.Identity.GetUserId();
            obj.Sm.UserId = userId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + "Transaction/PostSession");
                var postTask = client.PostAsJsonAsync(url + "Transaction/PostSession", obj);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ModelState.Clear();
                    TempData["success"] = "Security Session succeccfully updated...";
                    return RedirectToAction("Security");
                }
               
            }

             ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
          
            obj.s = Session["EmailStatus"] as sample;

            try { ViewBag.BTC = "$" + Math.Round(Convert.ToDecimal(objCoin.BTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.BTC = ex.Message; }
            try { ViewBag.ETH = "$" + Math.Round(Convert.ToDecimal(objCoin.ETHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETH = ex.Message; }
            try { ViewBag.DASH = "$" + Math.Round(Convert.ToDecimal(objCoin.DASHCurrentPrice()), 2); } catch (Exception ex) { ViewBag.DASH = ex.Message; }
            try { ViewBag.LTC = "$" + Math.Round(Convert.ToDecimal(objCoin.LTCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.LTC = ex.Message; }
            try { ViewBag.ETC = "$" + Math.Round(Convert.ToDecimal(objCoin.ETCCurrentPrice()), 2); } catch (Exception ex) { ViewBag.ETC = ex.Message; }
            try { ViewBag.MBC = "$" + Math.Round(Convert.ToDecimal(objCoin.GetMBC_USDCoin()), 2); } catch (Exception ex) { ViewBag.MBC = ex.Message; }

            return View("Security",obj);
        }

    }
}