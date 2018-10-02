using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Blockchaninaccounts.Models;
using System.Net;
using Newtonsoft.Json;
using Blockchaninaccounts.Controllers.APIcontroller;
using System.Data;
using Google.Authenticator;
using Blockchaninaccounts.Controllers.Dashboard;
using System.Net.NetworkInformation;
using Blockchaninaccounts.App_Start;

namespace Blockchaninaccounts.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get

            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //public string Get()
        //{
        //    HttpRequestBase baseRequest = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request as HttpRequestBase;

        //    HttpBrowserCapabilitiesBase browser = baseRequest.Browser;

        //    string b = browser.Browser + " " + browser.Version;

        //    // Here we can store the browser used in a data store (SqlServer, etc...).           

        //    return Ok();
        //}
        public string getLocation()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            string url = "http://freegeoip.net/json/" + ipAddress.ToString();
            WebClient client = new WebClient();
            string jsonstring = client.DownloadString(url);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonstring);
            System.Web.HttpContext.Current.Session["UserCountryCode"] = dynObj.country_code;
            string res1 = Session["UserCountryCode"] as string;
            return res1;
        }
        public string get()
        {
            //HttpBrowserCapabilities bc = new HttpBrowserCapabilities();
            // string a=bc.Browser;
            //string aa = getLocation();
            var ab = Request.Browser.Browser + " " + Request.Browser.Version;
            return ab;

        }
        public static string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return (sMacAddress);

        }
        public static string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        public static EmailNotification GetNotification(string id)
        {
            ValueController ob1 = new ValueController();
            DataSet ds = ob1.GetNotification(id);
           // Session["Notification"] = ds;
            EmailNotification en = new EmailNotification();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                en.send = (bool)dr[0];
                en.Receive = (bool)dr[1];
                en.Buy = (bool)dr[2];
                en.Sell = (bool)dr[3];
                en.Exchange=(bool)dr[4];

            }
     
            return en;
        }
      
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);   
            }
            ApplicationUser user;
            bool IsEmail = IsValidEmail(model.Email);
            if(IsEmail)
            {
                 user = await UserManager.FindByEmailAsync(model.Email);
            }
            else
            {
                user = await UserManager.FindByNameAsync(model.Email);
            }

            //Session["ID"] = Session.SessionID;
            //if (user != null)
            //{
            //    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            //    {
            //        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");
            //        ViewBag.errorMessage = "You must have a confirmed email to log on. please check your Email to confirm email.";
            //        return View("Error");
            //    }

            //}

            // Require the user to have a confirmed email before they can log on.
            //var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
               
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");
                    ViewBag.errorMessage = "You must have a confirmed email to log on.";
                    return View("Error");
                }
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Please Enter Correct Email or Username.");
                return View(model);

            }
            
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                    if (1 == 1)
                    {
                        string res = "Hi, you are login with " + get();
                       // SendMail a = new Blockchaninaccounts.App_Start.SendMail();
                        SendMail.Mail(user.Email,res);
                    }
                    ValueController vc = new ValueController();
                   var res1 = vc.PostAddress(user.Id);
                    var v = GetNotification(user.Id);
                    Session["Notification"] = v as EmailNotification;
                    SecurityCenterController.Email = user.Email;
                    SecurityCenterController.Id = user.Id;
                    string Ip = GetIPAddress();
                    string Mac = GetMacAddress();
                    string location = null;
                    Session["Email"] = user.Email;
                    Session["UserName"] = user.UserName;
                    if(user.GoogleAuthetication==true)
                    {
                        return RedirectToAction("GoogleCodeVarification");

                    }



                    return RedirectToLocal(returnUrl,user.Id,Ip,Mac,location,user.Email);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                    Session["Email"] = user.Email;
                    Session["UserName"] = user.UserName;
                    SecurityCenterController.Email = user.Email;
                    SecurityCenterController.Id = user.Id;
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Please Enter Correct Password.");
                        return View(model);
                }
           }
        public ActionResult GoogleCodeVarification()
        {
            //SecurityCenterController s = new SecurityCenterController();
            
            return View();
        }
        //public string GetKey()
        //{
        //    Random random = new Random();
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@)(*#";
        //    return new string(Enumerable.Repeat(chars, 13)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
        //public void GetQR()
        //{
        //    string key = GetKey();
        //    string Email1 = Email;
        //    bool status = true;
        //    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        //    string UserUniqueKey = Email1 + key; //as Its a demo, I have done this way. But you should use any encrypted value here which will be unique value per user 
        //    Session["xyz"] = UserUniqueKey;
        //    var setupInfo = tfa.GenerateSetupCode("Dotnet Awesome", Email1, UserUniqueKey, 300, 300);
        //    ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
        //    ViewBag.SetupCode = setupInfo.ManualEntryKey;
        //}

        //public ActionResult VarifyGoogle()
        //{
        //    GetQR();
        //    string returnUrl = null;
        //    var token = Request["passcode"];
        //    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        //    string UserUniqueKey = Session["xyz"].ToString();
        //    bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
        //    if (isValid)
        //    {
        //        Session["IsValid2FA"] = true;
        //        return RedirectToAction("RedirectToLocal",new { returnUrl });
        //    }
        //    return RedirectToAction("GoogleError", "Account");
        //}
        public ActionResult GoogleError()
        {
            TempData["GoogleError"] = 1;
            return RedirectToAction("GoogleCodeVarification", "Account");
        }
        // GET: /Account/VerifyCode
        //[AllowAnonymous]
        //public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        //{
        //    // Require that the user has already logged in via username/password or external login
        //    if (!await SignInManager.HasBeenVerifiedAsync())
        //    {
        //        return View("Error");
        //    }
        //    return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            if (!await SignInManager.SendTwoFactorCodeAsync(provider))
            {
                return View("Error");
            }
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    string id = SecurityCenterController.Id;
                    string Ip = GetIPAddress();
                    string Mac = GetMacAddress();
                    string Email = SecurityCenterController.Email;
                    string location = null;
                    return RedirectToLocal(model.ReturnUrl,id,Ip,Mac,location,Email);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            //    var result = await UserManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded)
            //    {
            //        //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

            //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //        // Send an email with this link
            //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //        await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //        return RedirectToAction("Index", "Home");
            //    }
            //    AddErrors(result);
            //}

            //// If we got this far, something failed, redisplay form
            //return View(model);
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    System.Collections.Generic.List<string> SecretPhase = UserPhase.SecretPhase();
                    //  Comment the following line to prevent log in until the user is confirmed.
                    //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    // var user1 = User.Identity.GetApplicationUser();
                    var user1 = await UserManager.FindByEmailAsync(model.Email);
                    foreach (var word in SecretPhase)
                    {
                        user1.SecretPhase = word;
                        UserManager.Update(user1);
                    }
                    //var result1 = await UserManager.UpdateAsync(user1);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                       new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account",
                       "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // Uncomment to debug locally 
                    // TempData["ViewBagLink"] = callbackUrl;

                    ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                                    + "before you can log in.";

                    return View("Info");
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
       
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                bool res = false;
                try
                {
                    ValueController vc = new ValueController();
                     res = vc.UserWalletInit(userId);
                }
               catch(Exception ex) { }
                if(res==false)
                {
                    var user1 = await UserManager.FindByIdAsync(userId);
                    user1.EmailConfirmed = false;
                    UserManager.Update(user1);
                    return View("Error");
                }
                return View("ConfirmEmail");
            }
            else return View("Error");
            //return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
              //  var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    //return View("ForgotPasswordConfirmation");
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);

                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry! This Email is not confirmed.");
                        return View(model);
                    }
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        //[AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //    {
        //        return View("Error");
        //    }
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            //var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            //return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            // return RedirectToAction("SendCode", new SendCodeViewModel { SelectedProvider = userFactors[0], ReturnUrl = returnUrl, RememberMe = rememberMe });
            return RedirectToAction("VerifyCode", new { Provider = userFactors[0], ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    string id = SecurityCenterController.Id;
                    return RedirectToLocal(returnUrl,id);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
       
        public ActionResult RedirectToLocal1(string Ip,string Mac,string location,string Email)
        {
           string id= User.Identity.GetUserId();
            return RedirectToLocal(null,id, Ip, Mac, location, Email);
        }
        private ActionResult RedirectToLocal(string returnUrl,string id=null,string Ip=null,string Mac=null,string location=null,string Email=null)
        {
            var SessionaData = ValueController.GetSession(id);
            if (Url.IsLocalUrl(returnUrl))
            {
                if (SessionaData.IPAdressWhiteList)
                {
                    var rx = CheckIpWhiteList(id, Ip, Mac, location);
                    if (rx == 0)
                    {
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return View("IPErrorPage");
                    }
                }
                    return Redirect(returnUrl);
            }
           // var userId = User.Identity.GetUserId();
            ValueController vc = new ValueController();
         
          
            if(SessionaData.DetectIP)
            {
                var res=ValueController.CheckIPAdress24(Ip, id);
                if(res==false)
                {
                    var data1 = ModelManager.PostSecurityData(id, Ip, Mac, location);
                    //SendMail obj = new SendMail();
                    SendMail.Mail(Email, "You are login with Ip Adress "+Ip+". Is it you!");
                }
            }
            else if(SessionaData.IPAdressWhiteList)
            {
               
                var rx=CheckIpWhiteList(id, Ip, Mac, location);
                if(rx==0)
                {
                 
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return View("IPErrorPage");
                }
            }
            var data = ModelManager.PostSecurityData(id, Ip, Mac, location);
            vc.PostSessionToken();
            vc.PostActivityLog(id, "Login", "Succeed");
            return RedirectToAction("Index", "Dashboard");
        }
        public int CheckIpWhiteList(string id,string Ip,string Mac,string location)
        {
            int count = 0;
            var res = ValueController.GetIpTrue(id);
            ValueController vc = new ValueController();
            if(res.Tables.Count!=0)
            {
                foreach (DataRow dr in res.Tables[0].Rows)
                {
                    if (Ip == dr[0].ToString())
                    {
                        count++;
                    }

                }
                if(count==0)
                {
                    vc.PostActivityLog(id, "Login", "Failed");
                    var data2 = ModelManager.PostSecurityData(id, Ip, Mac, location);
                    //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    //return View("IPErrorPage");
                }
            }
            return count;
        }
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }

           
        }
        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
        }
        #endregion
    }
}