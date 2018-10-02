using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Blockchaninaccounts.App_Start
{
    public class SendMail
    {
        public static void Mail(string useremail, string message)
        {


            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(useremail);
            myMessage.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);
            myMessage.Subject = "Email";
            myMessage.Html = message;

            var transportWeb = new SendGrid.Web(ConfigurationManager.AppSettings["MailAccountApi"]);
            //return View("ddd");

            if (transportWeb != null)
            {
                try
                {
                    transportWeb.DeliverAsync(myMessage);
                    // return View("Success");

                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                // Trace.TraceError("Failed to create Web transport.");
                // await Task.FromResult(0);
            }
            //return null;
        }
    }
}