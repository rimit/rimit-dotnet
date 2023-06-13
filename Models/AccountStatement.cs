using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rimit.Models
{
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Contents
    {
        public Datass data { get; set; }
    }

    public class Datass
    {
        public string country_code { get; set; }
        public string mobile { get; set; }
        public string account_number { get; set; }
        public string branch_code { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
    public class USER_ACCOUNT_DATA
    {
        public string balance_amount { get; set; }
        public int transactions_count { get; set; }
        public string account_number { get; set; }
        public string branch_code { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }

    public class ACCOUNT_TRANSACTION
    {
        public string txn_id { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string debit_amount { get; set; }
        public string credit_amount { get; set; }
        public string balance { get; set; }
        public string description { get; set; }
    }

    public class Headss
    {
        public string api { get; set; }
        public string apiVersion { get; set; }
        public string timeStamp { get; set; }
    }


    public class Headsss
    {
        public int HTTP_CODE { get; set; }
      
    }

    public class AccountStatement
    {
        public Headss head { get; set; }
        public Contents content { get; set; }
    }
    public class DatARequesting
    {
        public USER_ACCOUNT_DATA account { get; set; }
        public List<ACCOUNT_TRANSACTION> transactions { get; set; }
    }

}