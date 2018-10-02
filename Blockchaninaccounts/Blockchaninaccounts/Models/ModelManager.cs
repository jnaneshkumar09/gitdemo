using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace Blockchaninaccounts.Models
{
    public class ModelManager
    {
        public static string scn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static string GetAddress(string coinname,string userid)
        {
            string address = null;
            var list = GetTableName();
            using (SqlConnection con = new SqlConnection(scn))
            {
                foreach (var table in list)
                {
                    string CoinName = table.Split('_').Last();
                    SqlCommand cmd = new SqlCommand("select COUNT(*) from " + table, con);
                    con.Open();
                    int records = (int)cmd.ExecuteScalar();
                    if (records > 0)
                    {
                        if (coinname == CoinName)
                        {
                            SqlCommand cmd1 = new SqlCommand("select Address from " +table+ " where UserId='"+userid+"'", con);
                            address = (string)cmd1.ExecuteScalar();
                            return address;
                        }
                    }
                    con.Close();
                }
            }
            return address;
        }
        //if(coinname==CoinName)
        //            {
        //                SqlCommand cmd = new SqlCommand("", con);
        //address = (string)cmd.ExecuteScalar();
        //                return address;
        //            }
    public static List<string> GetTableName()
        {

            using (SqlConnection con = new SqlConnection(scn))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT name FROM sys.tables WHERE name like 'tbl_C%ADDRESS%'", con))
                {
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    List<string> ls = new List<string>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string name =string.Empty;
                        name = (string)dr["name"];
                        ls.Add(name);
                    }

                    return ls;
                }
            }
        }
        public bool PostAddress(string id)
        {
            bool Result = false;
           // string id = "123";
            var list = GetTableName();
            using (SqlConnection con = new SqlConnection(scn))
            {
                foreach(var data in list)
                {
                    SqlCommand cmd = new SqlCommand("select COUNT(*) from "+data, con);
                    con.Open();
                    int records = (int)cmd.ExecuteScalar();
                    if(records > 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("select COUNT(*) from " + data + " where UserId='"+id+"'", con);
                        int res1 = (int)cmd1.ExecuteScalar();
                        if(res1==0)
                        {
                            SqlCommand cmd2 = new SqlCommand("select min(KeyValue) from "+data+" where UserId is null", con);
                            string MinKeyValue = (string)cmd2.ExecuteScalar();
                            string KeyValue = MinKeyValue.Replace("'", "");
                            if(MinKeyValue != "")
                            {
                                SqlCommand cmd3 = new SqlCommand("update "+data+ " set UserId='"+id+"'"+ " where REPLACE(KeyValue,'''','')='" + KeyValue + "'", con);
                                int result=cmd3.ExecuteNonQuery();
                                Result = true;
                            }
                        }
                    }
                    con.Close();
                }
            }
            return Result;
        }
        public DataSet GetCoin()
        {
           
            using (SqlConnection con = new SqlConnection(scn))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, CoinName from tbl_Coin_Master", con))
                {
                    //con.Open();
                    //SqlDataReader dr = cmd.ExecuteReader();
                    //List<CoinList> ls = new List<CoinList>();
                    //while (dr.Read())
                    //{
                    //    CoinList ob = new CoinList();
                    //    ob.CoinId =(int)dr["CoinId"];
                    //    ob.CoinName = dr["CoinName"].ToString(); 
                    //    ls.Add(ob);
                    //}
                    //dr.Close();
                    //con.Close();
                    //return ls;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();

                    da.Fill(ds);

                    return ds;
                }
            }
        }
        public DataSet GetCountry()
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand com = new SqlCommand("Select * from Country", con);

                SqlDataAdapter da = new SqlDataAdapter(com);

                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
                
        }
        public DataSet GetState( int id)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand com = new SqlCommand("Select * from States where CountryId=@catid", con);

                com.Parameters.AddWithValue("@catid", id);

                SqlDataAdapter da = new SqlDataAdapter(com);

                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }

        }
        public DataSet GetCity( int id)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand com = new SqlCommand("Select * from City where StateId=@stateid", con);

                com.Parameters.AddWithValue("@stateid", id);

                SqlDataAdapter da = new SqlDataAdapter(com);

                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }

        }
        public bool LoginStatus( string id)
        {
            SqlConnection con = new SqlConnection(scn);

            SqlCommand cmd = new SqlCommand("", con);
                
                    cmd.Parameters.AddWithValue("@catid", id);
                  
                        con.Open();
                        int res=cmd.ExecuteNonQuery();
                      con.Close();
                        if (res>0)
                        {
                            return true;
                        }
                        else { return false; }

           }
        public DataSet GetNotification(string id)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand com = new SqlCommand("select Send,Receive, Buy,Sell, Exchange from tbl_EmailNotification  where UserId=@id", con);

                com.Parameters.AddWithValue("@id", id);

                SqlDataAdapter da = new SqlDataAdapter(com);

                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
        }
        public static DataSet GetSession(string id)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand com = new SqlCommand("select SendEmailOnLogin,DetectIpAddressChange,IpAddressWhiteList from tbl_SECURITY where UserId='"+id+"'", con);

                //com.Parameters.AddWithValue("@id", id);

                SqlDataAdapter da = new SqlDataAdapter(com);

                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
        }
        public static bool GetIpAdress(string id)//---get
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand com = new SqlCommand("select DISTINCT LoginIpAddress from tbl_USER_LOGIN_LOG where IsAuthorised=1 and LoginId=@id", con);

                com.Parameters.AddWithValue("@id", id);
                com.Parameters.AddWithValue("@ip", id);

                try
                {
                    int res = com.ExecuteNonQuery();
                    if(res==1)
                    return false;
                }
                catch(Exception ex) {}
                return false;
            }
        }
        public DataSet GetFeeType()
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand com = new SqlCommand("select FeeType from tbl_SendTransactionFee", con);

                SqlDataAdapter da = new SqlDataAdapter(com);

                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
        }
        public bool PostActivityLog(string Id,string Activity, string Result)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand("insert into tbl_Activity_Log values('"+Id+"',GETDATE(),'"+Activity+ "','" + Id + "','" + Result + "')", con);
                try
                {
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { throw ex; }
                finally
                { con.Close(); }
            }
            return false;
        }
        public bool ChangeGoogleStatus(string userid, bool value)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand("update AspNetUsers set GoogleAuthetication = '"+value+"' where Id = '"+userid+"'", con);
                try
                {
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { throw ex; }
                finally
                { con.Close(); }
            }
            return false;
        }
        public bool ChangeEmailStatus(string userid, bool value)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand("update AspNetUsers set TwoFactorEnabled = '" + value + "' where Id = '" + userid + "'", con);
                try
                {
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { throw ex; }
                finally
                { con.Close(); }
            }
            return false;
        }
        public bool GetGoogleStatus( string userid)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand("select GoogleAuthetication from AspNetUsers where id='" + userid + "'", con);
                try
                {
                    con.Open();
                    bool res = (bool)cmd.ExecuteScalar();
                    return res;
                }
                catch(Exception ex) { throw ex; }
                finally { con.Close(); }
            }
           // return false;
        }
        public decimal? GetFeeTypeValue(string coinid,string type)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand("select SendTransactionFeeNormal from tbl_Coin_Master where id='" + coinid + "'", con);
                SqlCommand cmd1 = new SqlCommand("select SendTransactionFeeReguler from tbl_Coin_Master where id='" + coinid + "'", con);
                SqlCommand cmd2 = new SqlCommand("select SendTransactionFeeHigh from tbl_Coin_Master where id='" + coinid + "'", con);
                con.Open();
                decimal res;
                try
                {
                    if (type == "Normal") {  res=(decimal)cmd.ExecuteScalar();return res; }
                    else if (type == "Regular") { res = (decimal)cmd1.ExecuteScalar();return res; }
                    else if (type == "High") { res = (decimal)cmd2.ExecuteScalar();return res; }
                }
                catch(Exception ex) { throw ex; }
                finally { con.Close(); }
            }
            return null;
        }
        public bool PostSessionToken(UserSessionModel obj)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd1 = new SqlCommand("SP_SessionToken", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@UserId", obj.UserId);
                cmd1.Parameters.AddWithValue("@Session", obj.Session);
                cmd1.Parameters.AddWithValue("@CreatedDate", obj.CreatedDate);
                cmd1.Parameters.AddWithValue("@ExpiryDate", obj.ExpiryDate);
               
                try
                {
                    con.Open();
                    int res = cmd1.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { throw ex; }
                finally
                { con.Close(); }
            }
            return false;
        }
        public static bool PostSecurityData(string UserId,string Ip,string Mac,string location,bool IsAuth=false)
        {
            string Id = Guid.NewGuid().ToString();
            DateTime date = DateTime.UtcNow;
            using (SqlConnection con = new SqlConnection(scn))
            {
                string Query = "";
                if (IsAuth == true)
                {
                    Query = "insert into tbl_USER_LOGIN_LOG(Id,LoginId,LoginDevice,LoginIpAddress,LoginLocation,CreatedDateTime,IsAuthorised) values('" + Id + "','" + UserId + "','" + Mac + "','" + Ip + "','" + location + "','" + date + "','"+IsAuth+"')";
                }
                else { Query = "insert into tbl_USER_LOGIN_LOG(Id,LoginId,LoginDevice,LoginIpAddress,LoginLocation,CreatedDateTime) values('" + Id + "','" + UserId + "','" + Mac + "','" + Ip + "','" + location + "','" + date + "')"; }
                SqlCommand cmd = new SqlCommand(Query, con);
                try
                {
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { throw ex; }
                finally
                { con.Close(); }
            }
            return false;
        }
        public static bool CheckIPAdress24( string ip,string userId)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand("select LoginIpAddress from tbl_USER_LOGIN_LOG where cast(CreatedDateTime as date)=CAST(GETDATE() as date) and LoginId='"+userId+"'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
               // if(ds.Container!=null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (ip == dr[0].ToString())
                        {
                            return true;
                        }
                    }
                }
               return false;
            }
        }
        public static DataSet GetIpTrue(string id)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand("select LoginIpAddress from tbl_USER_LOGIN_LOG where IsAuthorised=1 and LoginId='"+id+"'",con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        public bool UserWalletInit(string id)
        {
            DataSet ds = GetCoin();
            List<int> ls = new List<int>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int a = new int();
                a = (int)dr["Id"];
                ls.Add(a);
            }
            int co = ls.Count - 1;
            using (SqlConnection con = new SqlConnection(scn))
            {
                try
                {
                    con.Open();
                    foreach (var cid in ls)
                    {
                        SqlCommand cmd1 = new SqlCommand("InitialiseUserCoinBal", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@Uid", id);
                        cmd1.Parameters.AddWithValue("@CoinId", cid);
                        cmd1.Parameters.AddWithValue("@AvailableBalance", 0.0);
                        cmd1.Parameters.AddWithValue("@TotalBalance", 0.0);
                        cmd1.Parameters.AddWithValue("@CreatedUserId", id);
                        try { int res = cmd1.ExecuteNonQuery(); }
                        catch (Exception ex) { }
                    }
                    return true;
                }
                catch(Exception ex1) { }
                finally
                { con.Close(); }
                  
            }
            return false;
        }

        public static SettingProfileMode GetCutomerProfile(string userid)
        {
            using (SqlConnection con = new SqlConnection(scn))
            {
                SqlCommand cmd = new SqlCommand(" select * from tbl_PersonalDetails where UserId='" + userid + "'", con);
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    SettingProfileMode ob = new SettingProfileMode();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ob.Fname = dr["FirstName"].ToString();
                            ob.Lname = dr["LastName"].ToString();
                            ob.Mname = dr["MiddleName"].ToString();
                            ob.Nationality = dr["Nationality"].ToString();
                            ob.FullAddress = dr["Address"].ToString();
                            ob.Country = dr["Country"].ToString();
                            ob.State = dr["State"].ToString();
                            ob.City = dr["City"].ToString();
                            ob.DOB = Convert.ToDateTime(dr["DateOfBirth"]);
                        }
                        dr.Close();
                        return ob;
                    }
                    else
                    {
                        return ob;
                    }
                }
                catch (Exception ex) { }
                finally
                {
                    con.Close();
                    //return null; 
                }
            }
            return null;
        }
    }



}