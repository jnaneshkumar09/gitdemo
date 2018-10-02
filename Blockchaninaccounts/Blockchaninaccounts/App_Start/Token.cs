using Blockchaninaccounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blockchaninaccounts.App_Start
{
    public static class Token
    {
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