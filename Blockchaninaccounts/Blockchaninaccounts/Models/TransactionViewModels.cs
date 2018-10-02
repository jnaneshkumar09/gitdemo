using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blockchaninaccounts.Models
{
    public class TransactionViewModels
    {

    }

    public class CoinList
    {
        public int CoinId { set; get; }
        public string CoinName { set; get; }
    }
    public class EmailModel
    {
        public string UserId { set; get; }
        public bool value { set; get; }
    }
    public class UserSessionModel
    {
        public string Session { set; get; }
        public string UserId { set; get; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? ExpiryDate { set; get; }
    }
    public class UserSecurityModel
    {
        public string UserId { set; get; }
       // public bool
    }
    public class SendBitcoinViewModel
    {
        public string UserId { set; get; }
       // public List<CoinList> CoinList { set; get; }
        [Required(ErrorMessage = "Please select option")]
        public int? CoinTypeId { set; get; }
      //  [Required(ErrorMessage = "Please select option")]
        public string CoinName { set; get; }
        public string FromBitCoin { set; get; }
       // [Required(ErrorMessage = "Please enter Address")]
        public string ToBitCoin { set; get; }
        public decimal AmtInUSD { set; get; }
        [Required(ErrorMessage = "Enter number of coin to send")]
        public decimal AmtInCoins { set; get; }
        [Required(ErrorMessage = "Type a description about address")]
        public string DesAddress { set; get; }
        [Required(ErrorMessage = "please select options")]
        public string FeeType { set; get; }
        public DateTime? InsertDate { set; get; }
        public DateTime? UpdateDate { set; get; }
    }
    public class ExchangeViewMode
    {
        public string UserId { set; get; }
       // public List<CoinList> CoinList { set; get; }
        [Required(ErrorMessage = "Please select option")]
        public int ExchangeCoinId { set; get; }
        [Required(ErrorMessage = "Please select option")]
        public int ReceiveCoinId { set; get; }
        public string RCoinName { set; get; }
        public string ECoinName { set; get; }
        public decimal AmtInEx { set; get; }
        public decimal AmtInRe { set; get; }
        public DateTime? UpdateDate { set; get; }
        public DateTime? Inserted_Date { set; get; }
    }
    public class aa
    {
        public int CoinId { set; get; }
        public string CoinName { set; get; }
    }
    public class RecieveBitcoinViewModel
    {
       // public List<CoinList> CoinList { set; get; }
        public string UserId { set; get; }
        [Required(ErrorMessage = "Please select option")]
        public int? CoinTypeId { set; get; }
        public string CoinName { set; get; }
        public string QRCode { set; get; }
        public decimal AmtInUSD { set; get; }
        //[Required(ErrorMessage = "Enter number of coin to Receive")]
        public decimal AmtInCoins { set; get; }
        public string DestinationBitcoin { set; get; }
        [Required(ErrorMessage = "Please select option")]
        public string RecievedToEmail { set; get; }
        [Required(ErrorMessage = "Type a description about transaction")]
        public string TrasactionDes { set; get; }
        public DateTime? InsertDate { set; get; }
        public DateTime? UpdateDate { set; get; }
    }
    public class BuyViewModel
    {
       // public List<CoinList> CoinList { set; get; }
        public string UserId { set; get; }
        [Required(ErrorMessage = "Please select option")]
        public int? CoinTypeId { set; get; }
        public string CoinName { set; get; }
        public decimal AmtInUSD { set; get; }
        [Required(ErrorMessage = "Enter number of coin to send")]
        public decimal AmtInCoins { set; get; }
        public decimal Fee { set; get; }
        public decimal Tax { set; get; }
        public decimal Total { set; get; }
        public string PaymentType { set; get; }
        public string DestinationBitcoin { set; get; }
        public DateTime? InsertDate { set; get; }
        public DateTime? UpdateDate { set; get; }
    }
    public class SellViewModel
    {
      //  public List<CoinList> CoinList { set; get; }
        public string UserId { set; get; }
        [Required(ErrorMessage = "Please select option")]
        public int? CoinTypeId { set; get; }
        public string CoinName { set; get; }
        [Required(ErrorMessage = "USD amount required.")]
        public decimal AmtInUSD { set; get; }
        [Required(ErrorMessage = "Enter number of coin to send")]
        public decimal AmtInCoins { set; get; }
        [Required(ErrorMessage = "*")]
        public decimal Fee { set; get; }
        [Required(ErrorMessage = "*")]
        public decimal Tax { set; get; }
        [Required(ErrorMessage = "*")]
        public decimal Total { set; get; }
        public string PaymentDestination { set; get; }
        public DateTime? UpdateDate { set; get; }
        public DateTime? InsertedDate { set; get; }
    }
   
    public class City
    {
        public int CityId { set; get; }
        public string CityName { set; get; }
        public int StateId { set; get; }
    }
    public class State
    {
        public int StateId { set; get; }
        public string StateName { set; get; }
        public int CountryId { set; get; }
    }
    public class Country
    {
        public int CountryId { set; get; }
        public string CountryName { set; get; }
    }
    public class SettingViewMode
    {
       // public string UserId { set; get; }
        public SettingProfileMode profile { set; get; }
        public EmailNotification En { set; get; }
        public Logout lg { set; get; }
        public ChangePasswordViewModel cp { set; get; }
    }
    public class SettingProfileMode
    {
        public string UserId { set; get; }
        [Required(ErrorMessage = "First name must fill")]
        [RegularExpression("^[a-zA-Z]{3,}$", ErrorMessage = "First name must in Alpabets and atleast 3 character")]
        public string Fname { set; get; }
        [Required(ErrorMessage = "Middle name must fill")]
        [RegularExpression(@"^[a-zA-Z]{3,}$", ErrorMessage = "Middle name must in Alpabets atleast 3 character")]
        public string Mname { set; get; }
        [Required(ErrorMessage = "Last name must fill")]
        [RegularExpression(@"^[a-zA-Z]{3,}$", ErrorMessage = "Last name must in Alpabets atleast 3 character")]
        public string Lname { set; get; }
        [Required(ErrorMessage = "Please provide your date of birth.")]
        public DateTime DOB { set; get; }
        [Required(ErrorMessage = "Please provide your nationality.")]
        public string Nationality { set; get; }
        [Required(ErrorMessage = "Please provide your proper Address")]
        public string Address1 { set; get;}
        [Required(ErrorMessage = "Please provide your door number.")]
        public string DoorNo { set; get; }
        public string Address2 { set; get; }
        [Required(ErrorMessage = "Please Select the option")]
        public string Country { set; get; }
        public int Countryid { set; get;}
        [Required(ErrorMessage = "Please Select the option")]
        public string State { set; get; }
        [Required(ErrorMessage = "Please Select the option")]
        public string City { set; get; }
        [Required(ErrorMessage = "Please provide your city zipcode")]
        [RegularExpression(@"^\d{6,}$", ErrorMessage = "Zip code must be in number atleast 6 digits.")]
        public string Zipcode { set; get; }
        
        public string FullAddress { set; get; }
    }
    public class AllHistory
    {
        public string BuyCoin { set; get; }
        public decimal BuyAmount { set; get; }
        public DateTime Buydate { set; get; }
         public string SellCoin { set; get; }
        public decimal SellAmount { set; get; }
        public DateTime SellDate { set; get; }
        public string SendCoin { set; get; }
        public decimal SendAmount { set; get; }
        public DateTime SendDate { set; get; }
        public string ReceivedCoin { set; get; }
        public decimal ReceivedAmount { set; get; }
        public DateTime ReceivedDate { set; get; }
    }
    public class EmailNotification
    {
        public string UserId { set; get; }
       
        public bool send { set; get; }
        public bool Receive { set; get; }
        public bool Buy { set; get; }
        public bool Sell { set; get; }
        public bool Exchange { set; get; }


    }
    public class Logout
    {
        public string LogoutTime { set; get; }
    }
    public class BuyHistory
    {
        public string UserId { set; get; }
    }
    public class SecurityViewModel
    {
        public sample s { set; get; }
        public SessionModel Sm { set; get; }
    }
    public class sample
    {
        public int? no { set; get; }
        public bool TwoFactor { get; set; }
        public bool GoogleStatus { set; get; }
    }
    public class SessionModel
    {
        public string UserId { set; get; }
        public bool SendEmailOnLogin { set; get; }
        public bool DetectIP { set; get; }
        public bool IPAdressWhiteList { set; get; }
      
    }
}