using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using Blockchaninaccounts.Models;
using Microsoft.AspNet.Identity;
using System.Threading;
using Blockchaninaccounts.App_Start;

namespace Blockchaninaccounts.Controllers.APIcontroller
{
    [RoutePrefix("api/transaction")]
    public class TransactionController : ApiController
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //public IHttpActionResult GetSaveBuy(Guid UserId, int CoinTypeId, string DestinationofBitCoins, decimal Amount_In_USD, decimal Amount_In_Coins, decimal Fee, decimal Tax, decimal Total, string PaymentTYpe, DateTime Insert_Date)
        [HttpPost]
        [Route("Buy")]
        public IHttpActionResult PostSaveBuy(BuyViewModel b)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveBuy", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@UserId", b.UserId);
            cmd1.Parameters.AddWithValue("@CoinTypeId", b.CoinTypeId);
            cmd1.Parameters.AddWithValue("@DestinationofBitCoins", "XXX");
            cmd1.Parameters.AddWithValue("@Amount_In_USD", b.AmtInUSD);
            cmd1.Parameters.AddWithValue("@Amount_In_Coins", b.AmtInCoins);
            cmd1.Parameters.AddWithValue("@Fee", b.Fee);
            cmd1.Parameters.AddWithValue("@Tax", b.Tax);
            cmd1.Parameters.AddWithValue("@Total", b.Total);
            cmd1.Parameters.AddWithValue("@PaymentTYpe", "xxx");
            cmd1.Parameters.AddWithValue("@Insert_Date", System.DateTime.Now);

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            //int res=con.savaC(); 
           // if(res>0)                                                     
            if (Result != 0)
            {
                return Ok("Buy Amount Saved Successfully");
            }
            else
            {
                return BadRequest("Record not saved , Try again..");
            }
        }

        //public IHttpActionResult GetSaveSell(Guid UserId, int CoinTypeId, string DestinationofBitCoins, decimal Amount_In_USD, decimal Amount_In_Coins, decimal Fee, decimal Tax, decimal Total, string PaymentTYpe, DateTime Insert_Date)
        [HttpPost]
        [Route("Sell")]
        public IHttpActionResult PostSaveSell(SellViewModel s)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveSell", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@UserId", s.UserId);
            cmd1.Parameters.AddWithValue("@CoinTypeId", s.CoinTypeId);
            cmd1.Parameters.AddWithValue("@Amount_In_USD", s.AmtInUSD);
            cmd1.Parameters.AddWithValue("@Amount_In_Coins", s.AmtInCoins);
            cmd1.Parameters.AddWithValue("@Fee", s.Fee);
            cmd1.Parameters.AddWithValue("@Tax", s.Tax);
            cmd1.Parameters.AddWithValue("@Total", s.Total);
            cmd1.Parameters.AddWithValue("@PaymentDestination", "xxx");
            cmd1.Parameters.AddWithValue("@Inserted_Date", System.DateTime.Now);

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result != 0)
            {
                return Ok("Sell Amount Saved Successfully");
            }
            else
            {
                return Ok("Record not saved , Try again..");
            }
        }

        //public IHttpActionResult GetSaveReceive(Guid UserId, int CoinTypeId, string QRCode, string DestinationtoBitCoins, decimal Amount_In_USD, decimal Amount_In_Coins, string ReceivedTo, string DescriptionTrans, DateTime Insertdate)
         [HttpPost]

        //[ActionName("RecieveBitcoin")]
        [Route("ReceiveaPost")]
        public IHttpActionResult PostSaveReceive(RecieveBitcoinViewModel obj)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveReceive", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@UserId",obj.UserId);
            cmd1.Parameters.AddWithValue("@CoinTypeId", obj.CoinTypeId);
            cmd1.Parameters.AddWithValue("@QRCode",obj.QRCode);
            cmd1.Parameters.AddWithValue("@DestinationtoBitCoins", obj.DestinationBitcoin);
            cmd1.Parameters.AddWithValue("@Amount_In_USD", obj.AmtInUSD);
            cmd1.Parameters.AddWithValue("@Amount_In_Coins", obj.AmtInCoins);
            cmd1.Parameters.AddWithValue("@ReceivedTo", obj.RecievedToEmail);
            cmd1.Parameters.AddWithValue("@DescriptionTrans", obj.TrasactionDes);
            cmd1.Parameters.AddWithValue("@Insert_Date", System.DateTime.Now);

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result != 0)
            {
                return Ok("Received Amount Saved Successfully");
            }
            else
            {
                return Ok("Record not saved , Try again..");
            }
        }

        //public IHttpActionResult GetSaveSend(Guid UserId, int CoinTypeId, string QRCode, string BitCoinsFrom, string DestinationtoBitCoins, decimal Amount_In_USD, decimal Amount_In_Coins, string DescriptionAddress, string FeeTYpe, DateTime Insertdate)
        [HttpPost]
        [Route("SendPost")]
        public IHttpActionResult PostSaveSend(string sessiontoken, SendBitcoinViewModel obj)
        {
            int Result;
            //obj.UserId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
           // var res = Token.IsTokenValid(sessiontoken);
            //if(res==false)
            //{
            //    Redirect("~/Error.cshtml");
            //}
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveSend", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            //cmd1.Parameters.AddWithValue("@UserId", UserId);
            //cmd1.Parameters.AddWithValue("@CoinTypeId", CoinTypeId);
            //cmd1.Parameters.AddWithValue("@BitCoinsFrom", BitCoinsFrom);
            //cmd1.Parameters.AddWithValue("@DestinationtoBitCoins", DestinationtoBitCoins);
            //cmd1.Parameters.AddWithValue("@Amount_In_USD", Amount_In_USD);
            //cmd1.Parameters.AddWithValue("@Amount_In_Coins", Amount_In_Coins);
            //cmd1.Parameters.AddWithValue("@DescriptionAddress", DescriptionAddress);
            //cmd1.Parameters.AddWithValue("@FeeTYpe", FeeTYpe);
            //cmd1.Parameters.AddWithValue("@Insert_Date", Insertdate);

            obj.ToBitCoin = "aaaa";
            obj.FromBitCoin = "Null";
            cmd1.Parameters.AddWithValue("@UserId",obj.UserId);
            cmd1.Parameters.AddWithValue("@CoinTypeId",obj.CoinTypeId);
            cmd1.Parameters.AddWithValue("@BitCoinsFrom",obj.FromBitCoin);
            cmd1.Parameters.AddWithValue("@DestinationtoBitCoins",obj.ToBitCoin);
            cmd1.Parameters.AddWithValue("@Amount_In_USD",obj.AmtInUSD);
            cmd1.Parameters.AddWithValue("@Amount_In_Coins",obj.AmtInCoins);
            cmd1.Parameters.AddWithValue("@DescriptionAddress", obj.DesAddress);
            cmd1.Parameters.AddWithValue("@FeeTYpe", obj.FeeType);
            cmd1.Parameters.AddWithValue("@Insert_Date",System.DateTime.Now);

            try { Result = cmd1.ExecuteNonQuery(); }
            catch (Exception e) { throw e; }
            
                 con.Close();



            if (Result != 0  )
            {
                return Ok("Send Amount Saved Successfully");
            }
            else
            {
                return BadRequest("Record not saved , Try again..");
            }
        }
        [HttpPost]
        [Route("Exchange")]
        //public IHttpActionResult GetSaveExchange(Guid UserId, int CoinTypeId, decimal Amount_In_USD, decimal Amount_In_Coins, DateTime Updated_Date)
        public IHttpActionResult PostSaveExchange(ExchangeViewMode ex)
        {
            int Result = 0;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveExchange", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@UserId", ex.UserId);
            cmd1.Parameters.AddWithValue("@ExchangeCoinTypeId", ex.ExchangeCoinId);
            cmd1.Parameters.AddWithValue("@ReceiveCoinTypeId", ex.ReceiveCoinId);
            cmd1.Parameters.AddWithValue("@Amount_In_Ex", ex.AmtInEx);
            cmd1.Parameters.AddWithValue("@Amount_In_Re", ex.AmtInRe);
            cmd1.Parameters.AddWithValue("@Inserted_Date", System.DateTime.Now);
            try { Result = cmd1.ExecuteNonQuery(); }
            catch(Exception e) { throw e; }
            con.Close();
            if (Result != 0)
            {
                return Ok("Exchange Amount Saved Successfully");
            }
            else
            {
                return BadRequest("Record not saved , Try again..");
            }
        }

        public IHttpActionResult GetSaveSecurity(Guid UserId, int CoinTypeId, string QRCode, string BitCoinsFrom, string DestinationtoBitCoins, decimal Amount_In_USD, decimal Amount_In_Coins, string DescriptionAddress, string FeeTYpe, DateTime Insertdate)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd1 = new SqlCommand("tbl_USD_Coin_Send", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@UserId", UserId);
            cmd1.Parameters.AddWithValue("@CoinTypeId", CoinTypeId);
            cmd1.Parameters.AddWithValue("@BitCoinsFrom", BitCoinsFrom);
            cmd1.Parameters.AddWithValue("@DestinationtoBitCoins", DestinationtoBitCoins);
            cmd1.Parameters.AddWithValue("@Amount_In_USD", Amount_In_USD);
            cmd1.Parameters.AddWithValue("@Amount_In_Coins", Amount_In_Coins);
            cmd1.Parameters.AddWithValue("@FeeTYpe", FeeTYpe);
            cmd1.Parameters.AddWithValue("@DescriptionAddress", DescriptionAddress);
            cmd1.Parameters.AddWithValue("@Insert_Date", Insertdate);

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result == 0)
            {
                return Ok("Security details Saved Successfully");
            }
            else
            {
                return Ok("Record not saved , Try again..");
            }
        }

        public IHttpActionResult GetSaveUserActivation(Guid UserId, Guid ActivationCode)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_UserActivation", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@UserId", UserId);
            cmd1.Parameters.AddWithValue("@ActivationCode", ActivationCode);

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result == 0)
            {
                return Ok("User Activation Saved Successfully");
            }
            else
            {
                return Ok("Record not saved , Try again..");
            }
        }
        [HttpPost]
        [Route("Profile")]
        //public IHttpActionResult GetSavePersonalDetails(Guid UserId, string FirstName, string LastName, DateTime Registration_Date, string City, string Country, string ContactNumber, DateTime DateOfBirth, string Address, string Status)
        public IHttpActionResult PostSavePersonalDetails(SettingViewMode obj)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            string address = obj.profile.DoorNo+ "," + obj.profile.Address1+","+ obj.profile.Address2 +","+ obj.profile.Zipcode;
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_PersonalDetails", con);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@UserId",obj.profile.UserId);
            cmd1.Parameters.AddWithValue("@FirstName", obj.profile.Fname);
            cmd1.Parameters.AddWithValue("@MiddleName", obj.profile.Mname);
            cmd1.Parameters.AddWithValue("@LastName", obj.profile.Lname);
            cmd1.Parameters.AddWithValue("@Nationality", obj.profile.Nationality);
            cmd1.Parameters.AddWithValue("@Registration_Date", System.DateTime.Now);
            cmd1.Parameters.AddWithValue("@Country", obj.profile.Country);
            cmd1.Parameters.AddWithValue("@State", obj.profile.State);
            cmd1.Parameters.AddWithValue("@City", obj.profile.City);
            cmd1.Parameters.AddWithValue("@ContactNumber", "123456");
            cmd1.Parameters.AddWithValue("@DateOfBirth", obj.profile.DOB);
            cmd1.Parameters.AddWithValue("@Address", address);
            cmd1.Parameters.AddWithValue("@Status", "Active");

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result != 0)
            {
                return Ok("Personal Details Saved Successfully");
            }
            else
            {
                return Ok("Record not saved , Try again..");
            }
        }
        [HttpPost]
        [Route("Email")]
        public IHttpActionResult PostSaveEmailNotification(SettingViewMode obj)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();  
            }
           
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveEmailNotification", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@UserId", obj.En.UserId);
           
            cmd1.Parameters.AddWithValue("@Send", obj.En.send);
            cmd1.Parameters.AddWithValue("@Receive", obj.En.Receive);
            cmd1.Parameters.AddWithValue("@Buy", obj.En.Buy);
            cmd1.Parameters.AddWithValue("@Sell", obj.En.Sell);
            cmd1.Parameters.AddWithValue("@Exchange", obj.En.Exchange);
            cmd1.Parameters.AddWithValue("@TransactionDate", System.DateTime.Now);

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result != 0)
            {
                return Ok("Email Notification Saved Successfully");
            }
            else
            {
                return Ok("Record not saved , Try again..");
            }
        }
        [Route("PostSession")]
        public IHttpActionResult PostSession(SecurityViewModel obj)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveSecurityNotification", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@UserId", obj.Sm.UserId);
            cmd1.Parameters.AddWithValue("@SendEmailOnLogin", obj.Sm.SendEmailOnLogin);
            cmd1.Parameters.AddWithValue("@DetectIpAddressChange", obj.Sm.DetectIP);
            cmd1.Parameters.AddWithValue("@IpAddressWhiteList", obj.Sm.IPAdressWhiteList);
            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result != 0)
            {
                return Ok("Successfully");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        public IHttpActionResult GetSaveKYCdetails(Guid UserId, string Emailid, string AddressProofID, string DL_P_Number, Byte[] Address_FrontPage, Byte[] Address_LastPage, string IDProof, string ID_CardNumber, Byte[] IDFrontPage, Byte[] IDLastPage)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            string str = "Select count(*) from My_KYCDetails where EmailID='" + UserId + "'";
            DataTable dt1 = new DataTable();
            SqlDataAdapter sqlda = new SqlDataAdapter(str, con);
            DataSet ds1 = new DataSet();
            con.Open();
            sqlda.Fill(ds1);
            dt1 = ds1.Tables[0];
            con.Close();
            if (Convert.ToInt32(dt1.Rows[0][0].ToString()) > 0)
            {
                return Ok("KYC Details already Uploaded for this User");
            }

            //insert the file into database

            string strQuery = "insert into My_KYCDetails(Userid,EmailID,AddressProofID,DL_P_Number,Address_FrontPage,Address_LastPage,IDProof,ID_CardNumber,ID_FrontPage,ID_LastPage,InsertDate,isapproved)" +

               " values (@Userid,@EmailID,@AddressProofID,@DL_P_Number,@Address_FrontPage,@Address_LastPage,@IDProof,@ID_CardNumber,@ID_FrontPage,@ID_LastPage,@Insertdate,@isapproved)";

            SqlCommand cmd = new SqlCommand(strQuery, con);

            cmd.Parameters.Add("@Userid", SqlDbType.VarChar).Value = UserId;

            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar).Value = Emailid;

            cmd.Parameters.Add("@AddressProofID", SqlDbType.VarChar).Value = AddressProofID;

            cmd.Parameters.Add("@DL_P_Number", SqlDbType.VarChar).Value = DL_P_Number;

            cmd.Parameters.Add("@Address_FrontPage", SqlDbType.Binary).Value = Address_FrontPage;
            cmd.Parameters.Add("@Address_LastPage", SqlDbType.Binary).Value = Address_LastPage;

            cmd.Parameters.Add("@IDProof", SqlDbType.VarChar).Value = IDProof;

            cmd.Parameters.Add("@ID_CardNumber", SqlDbType.VarChar).Value = ID_CardNumber;

            cmd.Parameters.Add("@ID_FrontPage", SqlDbType.Binary).Value = IDFrontPage;
            cmd.Parameters.Add("@ID_LastPage", SqlDbType.Binary).Value = IDLastPage;
            cmd.Parameters.Add("@isapproved", SqlDbType.Int).Value = 0;
            DateTime Idt = new DateTime();
            Idt = DateTime.Now;
            cmd.Parameters.Add("@Insertdate", SqlDbType.DateTime).Value = Convert.ToDateTime(Idt);

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok("File Uploaded Successfully");
            }
            catch (Exception ex)
            {
                return Ok("File Upload Error");
            }
        }

        public IHttpActionResult GetSaveLoginLog(Guid LoginId, string LoginDevice, string LoginIpAddress, string LoginLocation, bool IsAuthorised)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SP_SaveLoginLog", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@LoginId", LoginId);
            cmd1.Parameters.AddWithValue("@LoginDevice", LoginDevice);
            cmd1.Parameters.AddWithValue("@LoginIpAddress", LoginIpAddress);
            cmd1.Parameters.AddWithValue("@LoginLocation", LoginLocation);
            cmd1.Parameters.AddWithValue("@IsAuthorised", IsAuthorised);

            int Result = cmd1.ExecuteNonQuery();
            con.Close();
            if (Result == 0)
            {
                return Ok("Login log Saved Successfully");
            }
            else
            {
                return Ok("Record not saved , Try again..");
            }
        }

        public IHttpActionResult GetKYCdetailsforUserId(string Userid)
        {
            try
            {
                string str = "SELECT UserId,EmailID,AddressProofID,DL_P_Number,Address_FrontPage,Address_LastPage,IDProof,ID_CardNumber,ID_FrontPage,ID_LastPage,Approveddate FROM My_KYCDetails where isapproved=1 and  UserId = '" + Userid + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                con.Close();
                string jsonstring = JsonConvert.SerializeObject(dt);
                return Ok(jsonstring);
            }
            catch (Exception ex)
            {
                return Ok("Records not found");
            }
        }

        public IHttpActionResult GetAllKYCdetails()
        {
            try
            {
                string str = "SELECT UserId,EmailID,AddressProofID,DL_P_Number,Address_FrontPage,Address_LastPage,IDProof,ID_CardNumber,ID_FrontPage,ID_LastPage,Approveddate FROM My_KYCDetails where isapproved=1";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                con.Close();
                string jsonstring = JsonConvert.SerializeObject(dt);
                return Ok(jsonstring);
            }
            catch (Exception ex)
            {
                return Ok("Records not found");
            }
        }

        public IHttpActionResult GetPersonalDetailsforUserId(string userid)
        {
            try
            {
                string str = "SELECT UserId, FirstName, LastName, Registration_Date, City, Country, ContactNumber, DateOfBirth, Address, Status FROM tbl_PersonalDetails where userid='" + userid + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                con.Close();
                string jsonstring = JsonConvert.SerializeObject(dt);
                return Ok(jsonstring);
            }
            catch (Exception ex)
            {
                return Ok("Records not found");
            }
        }

        public IHttpActionResult GetCustomerDetails()
        {
            try
            {
                string str = "SELECT UserId, FirstName, LastName, Registration_Date, City, Country, ContactNumber, DateOfBirth, Address, Status FROM tbl_PersonalDetails";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                con.Close();
                string jsonstring = JsonConvert.SerializeObject(dt);
                return Ok(jsonstring);
            }
            catch (Exception ex)
            {
                return Ok("Records not found");
            }
        }
        
        public IHttpActionResult GetExchangeDetails()
        {
            try
            {
                string str = "SELECT UserId,CoinTypeId,Amount_In_USD,Amount_In_Coins,Updated_Date FROM tbl_USD_Coin_Exchange";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                con.Close();
                string jsonstring = JsonConvert.SerializeObject(dt);
                return Ok(jsonstring);
            }
            catch (Exception ex)
            {
                return Ok("Records not found");
            }
        }

        public IHttpActionResult GetAllBuySellSendReceive()
        {
            try
            {
                string str = "SELECT (select coinname from tbl_CoinMaster where CoinId=a.CoinTypeId) as BuyCoin,a.Total as BuyAmount, a.Insert_Date as BuyDate  , (select coinname from tbl_CoinMaster where CoinId=b.CoinTypeId) as SellCoin,b.Total as SellAmount, b.Updated_Date as SellDate, (select coinname from tbl_CoinMaster where CoinId=c.CoinTypeId) as SendCoin,c.Amount_In_Coins as SendAmount, c.Updated_Date as SendDate, (select coinname from tbl_CoinMaster where CoinId=d.CoinTypeId) as ReceivedCoin,d.Amount_In_Coins as ReceivedAmount, c.Updated_Date as ReceivedDate FROM tbl_USD_Coin_Buy a, tbl_USD_Coin_Sell b, tbl_USD_Coin_Send c, tbl_USD_Coin_Receive d where a.UserId=b.UserId and b.UserId=c.UserId and c.UserId=d.UserId";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                con.Close();
                string jsonstring = JsonConvert.SerializeObject(dt);
                return Ok(jsonstring);
            }
            catch (Exception ex)
            {
                return Ok("Records not found");
            }
        }
        // [HttpGet]
        [System.Web.Http.Route("Getid")]
        public IHttpActionResult GetBuySellSendReceiveforUserId(string id)
        {
           
           
                string str = "SELECT (select coinname from tbl_CoinMaster where CoinId=a.CoinTypeId) as BuyCoin,a.Total as BuyAmount, a.Insert_Date as BuyDate  , (select coinname from tbl_CoinMaster where CoinId=b.CoinTypeId) as SellCoin,b.Total as SellAmount, b.Updated_Date as SellDate, (select coinname from tbl_CoinMaster where CoinId=c.CoinTypeId) as SendCoin,c.Amount_In_Coins as SendAmount, c.Updated_Date as SendDate, (select coinname from tbl_CoinMaster where CoinId=d.CoinTypeId) as ReceivedCoin,d.Amount_In_Coins as ReceivedAmount, c.Updated_Date as ReceivedDate FROM tbl_USD_Coin_Buy a, tbl_USD_Coin_Sell b, tbl_USD_Coin_Send c, tbl_USD_Coin_Receive d where a.UserId=b.UserId and b.UserId=c.UserId and c.UserId=d.UserId and a.userid='" + id + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
            if (ds.Container != null)
            {
                DataTable dt = new DataTable();

                dt = ds.Tables[0];


                string jsonstring = JsonConvert.SerializeObject(dt);
                return Ok(jsonstring);
            }
            else return NotFound();
        
        }
        [Route("GetBuy")]
        public IHttpActionResult GetBuyforUserId(Guid userid)
        {
            
                string str = "SELECT UserId,(select CoinName from tbl_Coin_Master where Id=CoinTypeId) as Coin_Name,DestinationofBitCoins,Amount_In_USD,Amount_In_Coins,Fee,Tax,Total,PaymentTYpe,Insert_Date,Updated_Date FROM tbl_USD_Coin_Buy where userid='" + userid + "'";
                SqlCommand cmd = new SqlCommand(str, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<BuyViewModel> ls = new List<BuyViewModel>();
                while (dr.Read())
                {
                    BuyViewModel ob = new BuyViewModel();
                    ob.UserId = dr[0].ToString();
                    ob.CoinName = (string)dr[1];
                    ob.DestinationBitcoin = (string)dr[2];
                    ob.AmtInUSD = (decimal)dr[3];
                    ob.AmtInCoins = (decimal)dr[4];
                    ob.Fee = (decimal)dr[5];
                    ob.Tax = (decimal)dr[6];
                    ob.Total = (decimal)dr[7];
                    ob.PaymentType = (string)dr[8];
                    ob.InsertDate = (DateTime)dr[9];
                    if (dr[10] == DBNull.Value)
                        ob.UpdateDate = null;
                    else
                        ob.UpdateDate = (DateTime)dr[10];
                    ls.Add(ob);
                }
                dr.Close();
                con.Close();
                //SqlDataAdapter da = new SqlDataAdapter(str, con);
                //DataSet ds = new DataSet();
                //da.Fill(ds);
                //DataTable dt = new DataTable();
                //dt = ds.Tables[0];
                //con.Close();
                //string jsonstring = JsonConvert.SerializeObject(dt);
                //return Ok(jsonstring);
                if (ls.Count == 0)
                {
                    return NotFound();
                }

                return Ok(ls);
                //}
                //catch (Exception ex)
                //{
                //    return Ok("Records not found");
                //}
            }
        [Route("GetSell")]
        public IHttpActionResult GetSellforUserId(Guid userid)
        {
           
                string str = "SELECT UserId,(select CoinName from tbl_Coin_Master where Id=CoinTypeId) as Coin_Name,Amount_In_USD,Amount_In_Coins,Fee,Tax,Total,PaymentDestination,Inserted_Date FROM tbl_USD_Coin_Sell where UserId='" + userid + "'";
                SqlCommand cmd = new SqlCommand(str, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<SellViewModel> ls = new List<SellViewModel>();
                while (dr.Read())
                {
                    SellViewModel ob = new SellViewModel();
                    ob.UserId = dr[0].ToString();
                    ob.CoinName = (string)dr[1];
                    ob.AmtInUSD = (decimal)dr[2];
                    ob.AmtInCoins = (decimal)dr[3];
                    ob.Fee = (decimal)dr[4];
                    ob.Tax = (decimal)dr[5];
                    ob.Total = (decimal)dr[6];
                    ob.PaymentDestination = (string)dr[7];
                   // ob.InsertDate = (DateTime)dr[9];
                    if (dr[8] == DBNull.Value)
                        ob.InsertedDate = null;
                    else
                        ob.InsertedDate = (DateTime)dr[8];
                    ls.Add(ob);
                }
                dr.Close();
                con.Close();
                //SqlDataAdapter da = new SqlDataAdapter(str, con);
                //DataSet ds = new DataSet();
                //da.Fill(ds);
                //DataTable dt = new DataTable();
                //dt = ds.Tables[0];
                //con.Close();
                //string jsonstring = JsonConvert.SerializeObject(dt);
                //return Ok(jsonstring);
                if (ls.Count == 0)
                {
                    return NotFound();
                }

                return Ok(ls);
                //}
                //catch (Exception ex)
                //{
                //    return Ok("Records not found");
                //}
            }
        [Route("Getsend")]
        public IHttpActionResult GetSendforUserId(Guid userid)
        {
            
                string str = "SELECT UserId,(select CoinName from tbl_Coin_Master where Id=CoinTypeId) as Coin_Name,BitCoinsFrom,DestinationtoBitCoins,Amount_In_USD,Amount_In_Coins,DescriptionAddress,FeeTYpe,Insert_Date,Updated_Date FROM tbl_Coin_Send where UserId='" + userid + "'";
                // SqlDataAdapter da = new SqlDataAdapter(str, con);
                SqlCommand cmd = new SqlCommand(str, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<SendBitcoinViewModel> ls = new List<SendBitcoinViewModel>();
                while (dr.Read())
                {
                    SendBitcoinViewModel ob = new SendBitcoinViewModel();
                    ob.UserId = dr[0].ToString();
                    ob.CoinName = (string)dr[1];
                    ob.FromBitCoin = (string)dr[2];
                    ob.ToBitCoin = (string)dr[3];
                    ob.AmtInUSD = (decimal)dr[4];
                    ob.AmtInCoins = (decimal)dr[5];
                    ob.DesAddress = (string)dr[6];
                    ob.FeeType = (string)dr[7];
                    ob.InsertDate = (DateTime)dr[8];
                if (dr[9] == DBNull.Value)
                    ob.UpdateDate = null;
                else
                    ob.UpdateDate = (DateTime)dr[9];
                ls.Add(ob);
                }
                dr.Close();
                con.Close();
                //return ls;
               // return Ok(ls);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //DataTable dt = new DataTable();
            //dt = ds.Tables[0];
            //con.Close();
            //string jsonstring = JsonConvert.SerializeObject(dt);
            //return Ok(jsonstring);
            if (ls.Count == 0||ls==null)
            {
                return NotFound();
            }

            return Ok(ls);
        }
        [Route("GetRec")]
        public IHttpActionResult GetReceiveforUserId(Guid userid)
        {

            string str = "SELECT UserId,(select CoinName from tbl_Coin_Master where Id=CoinTypeId) as Coin_Name,QRCode,DestinationtoBitCoins,Amount_In_USD,Amount_In_Coins,ReceivedTo,DescriptionTrans,Insert_Date,Updated_Date FROM tbl_Coin_Receive where UserId='" + userid + "'";
                SqlCommand cmd = new SqlCommand(str, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<RecieveBitcoinViewModel> ls = new List<RecieveBitcoinViewModel>();
                while (dr.Read())
                {
                    RecieveBitcoinViewModel ob = new RecieveBitcoinViewModel();
                    ob.UserId = dr[0].ToString();
                    ob.CoinName = (string)dr[1];
                    ob.QRCode = (string)dr[2];
                    ob.DestinationBitcoin = (string)dr[3];
                    ob.AmtInUSD = (decimal)dr[4];
                    ob.AmtInCoins = (decimal)dr[5];
                    ob.RecievedToEmail = (string)dr[6];
                    ob.TrasactionDes = (string)dr[7];
                    ob.InsertDate = (DateTime)dr[8];
                if (dr[9] ==DBNull.Value)
                    ob.UpdateDate = null;
                else
                    ob.UpdateDate = (DateTime)dr[9];
                    ls.Add(ob);
                }
                dr.Close();
                con.Close();
                //SqlDataAdapter da = new SqlDataAdapter(str, con);
                //DataSet ds = new DataSet();
                //da.Fill(ds);
                //DataTable dt = new DataTable();
                //dt = ds.Tables[0];
                //con.Close();
                //string jsonstring = JsonConvert.SerializeObject(dt);
                //return Ok(jsonstring);
                if (ls.Count == 0)
                {
                    return NotFound();
                }

                return Ok(ls);
            }
        [Route("GetEx")]
        public IHttpActionResult GetExchangeforUserId(Guid userid)
        {
            //(select CoinName from tbl_Coin_Master where Id = ReceiveCoinTypeId) as RecivCoin_Name
            string str = "select (select CoinName from tbl_Coin_Master where Id = ExchangeCoinTypeId) as ECoin_Name,(select CoinName from tbl_Coin_Master where Id = ReceiveCoinTypeId) as RCoin_Name,Amount_In_Exchange_Coin,Amount_In_Receive_Coin,InsertedDate from tbl_Coin_Exchange where UserId='" + userid + "'";
            SqlCommand cmd = new SqlCommand(str, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            List<ExchangeViewMode> ls = new List<ExchangeViewMode>();
            while (dr.Read())
            {
                ExchangeViewMode ob = new ExchangeViewMode();
                ob.ECoinName = (string)dr[0];
                ob.RCoinName = (string)dr[1];
                ob.AmtInEx = (decimal)dr[2];
                ob.AmtInRe = (decimal)dr[3];
                if (dr[4] == DBNull.Value)
                    ob.Inserted_Date = null;
                else
                    ob.Inserted_Date = (DateTime)dr[4];
                ls.Add(ob);
            }
            dr.Close();
            con.Close();
            if (ls.Count == 0)
            {
                return NotFound();
            }

            return Ok(ls);
        }
    }
}