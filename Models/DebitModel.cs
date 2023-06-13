using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rimit.Models
{
   


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AuthenticationDebit
    {
        public string mode { get; set; }
        public string hash { get; set; }
        public string signature { get; set; }
    }

    public class ContentDebit
    {
        public DataDebit data { get; set; }
    }

    public class DataDebit
    {
        public User user { get; set; }
        public TransactionDebit transaction { get; set; }
        public SettlementDebit settlement { get; set; }
        public AuthenticationDebit authentication { get; set; }
    }

    public class HeadDebit
    {
        public string api { get; set; }
        public string apiVersion { get; set; }
        public string timeStamp { get; set; }
    }

    public class DebitModel
    {
        public HeadDebit head { get; set; }
        public ContentDebit content { get; set; }
    }

    public class SettlementDebit
    {
        public string account_type { get; set; }
        public string account_number { get; set; }
    }

    public class TransactionDebit
    {
        public string txn_number { get; set; }
        public string txn_urn { get; set; }
        public string txn_type { get; set; }
        public string txn_nature { get; set; }
        public string txn_note { get; set; }
        public string txn_date { get; set; }
        public string txn_time { get; set; }
        public string txn_ts { get; set; }
        public string txn_amount { get; set; }
        public string txn_service_charge { get; set; }
        public string txn_sp_charge { get; set; }
        public string txn_fee { get; set; }
    } 
    
    public class DEBIT_CONFIRM_DATA
    {
        public string txn_number { get; set; }
        public string txn_urn { get; set; }
        public string txn_type { get; set; }
        public string txn_nature { get; set; }
        public string TRANSACTION_REF { get; set; }
        public string txn_amount { get; set; }
        public string account_balance { get; set; }
    }

    public class User
    {
        public string country_code { get; set; }
        public string mobile { get; set; }
        public string branch_code { get; set; }
        public string account_number { get; set; }
        public string account_type { get; set; }
        public string account_class { get; set; }
    }


}