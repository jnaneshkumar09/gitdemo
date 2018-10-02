using Blockchaninaccounts.Controllers.APIcontroller;
using Blockchaninaccounts.Controllers.Dashboard;
using Blockchaninaccounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blockchaninaccounts.Controllers
{
   public class SessionTokenGenerationcs
    {
        public static string  session { get; set; }
        string Email = SecurityCenterController.Email;
        string id = SecurityCenterController.Id;
       // ValueController vc = new ValueController();
        public UserSessionModel Token(UserSessionModel obj)
        {
            session = Guid.NewGuid().ToString().Replace("-", string.Empty);
            obj.UserId = id;
            obj.Session = session;
            obj.CreatedDate = DateTime.UtcNow;
            obj.ExpiryDate= DateTime.UtcNow.AddDays(2);
           // vc.PostSessionToken(obj);
            return obj;

        }
        
        

}
}