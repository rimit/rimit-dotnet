using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace Rimit.Models
{
    

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Auth
    {
        public string API_ID { get; set; }
        public string API_KEY { get; set; }
    }

    public class EncryptedDatas
    {
        public string cipher_text { get; set; }
        public string iv { get; set; }
        public string hash { get; set; }
    }

    public class Datas
    {
        public USERDATA user { get; set; }
        public List<Account> accounts { get; set; }
    }

    public class Account
    {
        public string account_name { get; set; }
        public string account_number { get; set; }
        public string branch_code { get; set; }
        public string branch_name { get; set; }
        public string account_type { get; set; }
        public string account_class { get; set; }
        public string account_status { get; set; }
        public string account_opening_date { get; set; }
        public string account_currency { get; set; }
        public string account_daily_limit { get; set; }
        public bool is_debit_allowed { get; set; }
        public bool is_credit_allowed { get; set; }
        public bool is_cash_debit_allowed { get; set; }
        public bool is_cash_credit_allowed { get; set; }
        public string auth_salt { get; set; }
    }

   
    public class Heads
    {
        public string api { get; set; }
        public string apiVersion { get; set; }
        public string timeStamp { get; set; }
        public Auth auth { get; set; }
    }

    public class AddAccountModel
    {
        public Heads head { get; set; }
        public string encrypted_data { get; set; }
    }


}