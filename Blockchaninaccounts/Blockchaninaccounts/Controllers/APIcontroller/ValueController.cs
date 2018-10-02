using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;


namespace Blockchaninaccounts.Controllers.APIcontroller
{
    [System.Web.Http.RoutePrefix("api/value")]
    public class ValueController : ApiController
    {

        ModelManager ob = new ModelManager();
        //[HttpGet]
        [System.Web.Http.Route("GetCoin")]
        public DataSet GetCoin()
        {
            return ob.GetCoin();
        }
        public bool ChangeGoogleStatus(string userId, bool value)
        {
            // return ob.ChangeGoogleStatus(userid, value);
            bool res = ob.ChangeGoogleStatus(userId, value);
            //if(res==true && value==true)
            //{

            //}
            if (res == false)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format(" data not updated.")),
                    ReasonPhrase = "Data not updated",
                    //StatusCode = statusCode;
                };

                throw new HttpResponseException(response);
            }
            return res;
        }
        public bool ChangeEmailStatus(string userId, bool value)
        {
            // return ob.ChangeGoogleStatus(userid, value);
            bool res = ob.ChangeEmailStatus(userId, value);
            //if(res==true && value==true)
            //{

            //}
            if (res == false)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format(" data not updated.")),
                    ReasonPhrase = "Data not updated",
                    //StatusCode = statusCode;
                };

                throw new HttpResponseException(response);
            }
            return res;
        }
        public bool GetGoogleStatus(string userid)
        {
            try
            {
                return ob.GetGoogleStatus(userid);
            }
            catch (Exception ex) { throw ex; }

        }
        public static SettingProfileMode GetCutomerProfile(string userid)
        {
            try
            {
                var res = ModelManager.GetCutomerProfile(userid);

                return res;
            }
            catch(Exception ex) { throw ex; }
        }
        public static SessionModel GetSession(string id)
        {
            try
            {
                DataSet ds = ModelManager.GetSession(id);
                SessionModel sm = new SessionModel();
                //if(ds.Container!=null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sm.SendEmailOnLogin = (bool)dr[0];
                        sm.DetectIP = (bool)dr[1];
                        sm.IPAdressWhiteList = (bool)dr[2];
                    }
               }
                return sm;
            }
            catch (Exception ex) { throw ex; }
        }
        public static bool GetIpAdress(string id)
        {
            try { return ModelManager.GetIpAdress(id); }
            catch(Exception ex) { throw; }
        }
        public static DataSet GetIpTrue(string id)
        {
            return ModelManager.GetIpTrue(id);
        }
        public static bool CheckIPAdress24(string ip, string userId)
        {
           return ModelManager.CheckIPAdress24(ip, userId);
        }

        // [HttpGet]
        [System.Web.Http.Route("GetCountry")]
        public DataSet GetCountry()
        {
            DataSet ds = ob.GetCountry();
            return ds;
        }
        [System.Web.Http.Route("GetState")]
        public DataSet GetState(int id)
        {
            DataSet ds = ob.GetState(id);
            return ds;
        }
        [System.Web.Http.Route("GetCity")]
        public DataSet GetCity( int id)
        {
            DataSet ds = ob.GetCity(id);
            return ds;
        }
        public bool PostActivityLog(string Id, string Activity, string Result)
        {
            bool res = ob.PostActivityLog(Id, Activity, Result);
            return res;
        }
        public static bool PostSecurityData(string Id,string Ip,string Mac,string Location,bool IsAuth=false)
        {
            bool res = ModelManager.PostSecurityData(Id, Ip, Mac,Location,IsAuth);
            return res;
        }
       // [System.Web.Http.Route("PostChangePass")]
       // [ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePassword(SettingViewMode obj)
        //{
        //    //ManageController mc = new ManageController();
        //    Sample s = new Sample();
        //     return await s.ChangePassword(obj);
        //}
        [System.Web.Http.Route("GetLoginStatus")]
        public bool LoginStatus(string id)
        {
            
            bool res = ob.LoginStatus(id);
            return res;
        }
        [System.Web.Http.Route("PostUserWalletInit")]
        public bool UserWalletInit(string id)
        {
            return ob.UserWalletInit(id);
        }
        public bool PostAddress(string id)
        {
            return ob.PostAddress(id);
        }
        public static string GetAddress(string coinname, string userid)
        {
            return ModelManager.GetAddress(coinname, userid);
        }
        [System.Web.Http.Route("GetNotification")]
        public DataSet GetNotification(string id)
        {
            DataSet ds = ob.GetNotification(id);
            return ds;
        }
        [System.Web.Http.Route("GetFeeType")]
        public DataSet GetFeeType()
        {
            DataSet ds = ob.GetFeeType();
            return ds;
        }
        public decimal? GetFeeTypeValue(string coinid,string type,string sessiontoken)
        {
            decimal? res = 0;


            var tokenStatus = IsTokenValid(sessiontoken);
            if (tokenStatus)
            {
               res = ob.GetFeeTypeValue(coinid, type);
                return res;
            }
            else
            {
                return res;
            }
        }
        [System.Web.Http.Route("GetUserId")]
        public  string GetUserId()
        {
            string userid= User.Identity.GetUserId();
            return userid;
        }
        [System.Web.Http.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IHttpActionResult Get()
        {
            string userid = User.Identity.GetUserId();
            if(userid!=null)
            {
                return Ok(userid);
            }
            return BadRequest();
        }
        //protected Guid? UserId
        //{
        //    get
        //    {
        //        if (!Request.IsAuthenticated)
        //            return null;

        //        var claimsIdentity = HttpContext.User.Identity as System.Security.Claims.ClaimsIdentity;
        //        var claim = claimsIdentity?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        //        if (claim != null)
        //            return new Guid(claim.Value);

        //        return null;
        //    }
        //}

        public bool PostSessionToken()
        {
            SessionTokenGenerationcs sobj = new SessionTokenGenerationcs();
            UserSessionModel us = new UserSessionModel();
            var obj = sobj.Token(us);
            bool res = ob.PostSessionToken(obj);
            return res;
        }
        public static bool IsTokenValid(string sessionToken)
        {
            using (var db = new WalletDigixhubEntities())
            {
                var result = db.USER_SESSION.SingleOrDefault(x => x.SessionToken == sessionToken && x.ExpiryDate > DateTime.UtcNow);

                return result == null ? false : true;
            }
        }
    }

    
  
}
