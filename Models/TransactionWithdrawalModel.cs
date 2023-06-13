using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rimit.Models
{
    public class TransactionWithdrawalModel
    {
        public string txn_number { get; set; }
        public string txn_urn { get; set; }
        public string txn_type { get; set; }
        public string txn_nature { get; set; }
        public string txn_amount { get; set; }
    }
}