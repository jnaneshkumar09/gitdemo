using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Blockchaninaccounts.Controllers.APIcontroller
{
    [System.Web.Http.RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        public static string msg;
        public static string getmsg
        {
            get { return msg; }
            set { msg = value; }
        }
        public  UserController()
        {}
        public UserController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // POST api/User/ChangePassword
        [System.Web.Http.Route("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> ChangePassword(SettingViewMode model)
        {
            //string msg = "";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ValueController obj = new ValueController();
           // var userId = obj.GetUserId();
            IdentityResult result = await UserManager.ChangePasswordAsync(model.cp.UserId, model.cp.OldPassword,
                model.cp.NewPassword);

            if (!result.Succeeded)
            {
                // BadRequestErrorMessageResult re = new BadRequestErrorMessageResult();
                // var res=GetErrorResult(result);
                //return BadRequest(res.ToString());
                return GetErrorResult(result);
                //return BadRequest((result.);

                //if (result.Errors != null)
                //{
                //    foreach (string error in result.Errors)
                //    {
                //        // ModelState.AddModelError("", error);
                //        msg = error;
                       
                //    }
                //    return BadRequest(msg);
                //}
                //if (result == null)
                //{
                //    return InternalServerError();
                //}

               // return BadRequest(msg);
              
            }

            return Ok();
        }

        

        //public async Task<IHttpActionResult> Login()
        //{

        //}

        // POST api/User/Register
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Username, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                var user1 = await UserManager.FindByEmailAsync(model.Email);

                //var result1 = await UserManager.UpdateAsync(user1);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl1= this.Url.Link("Default", new { Controller = "Account", Action = "ConfirmEmail", userId = user.Id, code = code });
               
                //var callbackUrl = Url.Link("ConfirmEmail",
                //   new { userId = user.Id, code = code });
                await UserManager.SendEmailAsync(user.Id, "Confirm your account",
                   "Please confirm your account by clicking <a href=\"" + callbackUrl1 + "\">here</a>");
            }

           else if (!result.Succeeded)
            {
                return (GetErrorResult(result));
            }

            return Ok();
        }
        //public string Get()
        //{
        //    var url = this.Url.Link("Default", new { Controller = "Account", Action = "ConfirmEmail", param1 = 1, param2 = "somestring" });
        //    return url;
        //}
        //public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //       // return View("Error");
              
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            //string msg = "";
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                         ModelState.AddModelError("", error);
                        msg = error;
                       // msg = error;
                    }
                }

                if(result.Errors!=null)
                {

                }
                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        //public async Task<IHttpActionResult> EnableTwoFactorAuthentication()
        //{
        //    IdentityResult result = await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //    }
        //     // return RedirectToAction("Index", "Manage");


        //    if (!result.Succeeded)
        //    {

        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("PostEmailStatus")]
        public async Task<IHttpActionResult> EmailStatus(EmailModel em)
        {
            
            IdentityResult result = await UserManager.SetTwoFactorEnabledAsync(em.UserId, em.value);
            var user = await UserManager.FindByIdAsync(em.UserId);
            if (user != null)
            {
                //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }
            // return RedirectToAction("Index", "Manage");


            if (!result.Succeeded)
            {

                return GetErrorResult(result);
            }
            if(result.Succeeded && em.value==true)
            {
                ValueController obj = new ValueController();
                sample google2FA = new sample() { GoogleStatus = obj.GetGoogleStatus(em.UserId) };
                if (google2FA.GoogleStatus == true)
                {
                     obj.ChangeGoogleStatus(em.UserId, false);
                }
            }
            return Ok();
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }
    }
}
