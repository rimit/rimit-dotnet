using Rimit.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Utility;

namespace Rimit.Controllers
{
    [RoutePrefix("Api")]

    public class TransactionStatusController : ApiController
    {
        private Config config = new Config();

        


        [Route("TransactionStatus")]
        [HttpPost]
        public IHttpActionResult TransactionStatus( [FromBody()] TransactionWithdrawalModel req)
        {
            Rimit.Utility.Response res = new Rimit.Utility.Response();
            try
            {
                

                String AUTH_API_ID = ConfigurationManager.AppSettings["API_ID"]; ;
                String AUTH_API_KEY = ConfigurationManager.AppSettings["API_KEY"];
                /*  */

                // ADD_ACCOUNT REQUEST URL
                String DEBIT_CONFIRM_URL = config.BASE_URL + "/transaction/statusCheck";

                string ENCRYPTION_KEY = ConfigurationManager.AppSettings["ENCRYPTION_KEY"];


                Auth auth = new Auth();
                auth.API_ID = AUTH_API_ID;
                auth.API_KEY = AUTH_API_KEY;
                Heads TXN_STATUS_HEAD = new Heads();
                TXN_STATUS_HEAD.apiVersion = "statusCheck";
                TXN_STATUS_HEAD.api = "v1";
                TXN_STATUS_HEAD.timeStamp = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss a");
                TXN_STATUS_HEAD.auth = auth;
               
                /*  */
                /*  */
                /* VERIFY THE USER */
                /* MANAGE SCOPE FOR FAILED TRANSACTIONS (Refer - https://doc.rimit.co/transaction-debit/confirm-debit#result-code) */
                /* VERIFY THE USER ACCOUNT */
                /* VERIFY THE USER ACCOUNT BALANCE AVAILABILITY */
                /* DEBIT USER ACCOUNT WITH txn_amount */
                /*  */
                /*  */

                /*  */
                /* GENERATE A UNIQUE TRANSACTION_REF */
                string TRANSACTION_REF = "";
                /*  */

                /*  */
                /* ASSIGN LATEST ACCOUNT_BALANCE AFTER CREDITING THE TRANSACTION_AMOUNT */
                string ACCOUNT_BALANCE = "";
                /*  */

                var TRANSACTION = req;
                DEBIT_CONFIRM_DATA TXN_STATUS_DATA = new DEBIT_CONFIRM_DATA();
                TXN_STATUS_DATA.txn_urn = TRANSACTION.txn_urn;
                TXN_STATUS_DATA.txn_number = TRANSACTION.txn_number;
                TXN_STATUS_DATA.TRANSACTION_REF = TRANSACTION_REF;
                TXN_STATUS_DATA.txn_amount = TRANSACTION.txn_amount;
                TXN_STATUS_DATA.txn_type = TRANSACTION.txn_type;
                TXN_STATUS_DATA.txn_nature = TRANSACTION.txn_nature;
                TXN_STATUS_DATA.account_balance = ACCOUNT_BALANCE;


                ResponseModel results = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,
                };
                var DEBIT_CONFIRM_RESULT = new JavaScriptSerializer().Serialize(results);
                var _head = new JavaScriptSerializer().Serialize(TXN_STATUS_HEAD);
                var _dEBIT_CONFIRM_DATA = new JavaScriptSerializer().Serialize(TXN_STATUS_DATA);


                Request request_ = new Request();
                var DEBIT_CONFIRM = request_.confirmRequest(_head, DEBIT_CONFIRM_RESULT, _dEBIT_CONFIRM_DATA, DEBIT_CONFIRM_URL, ENCRYPTION_KEY);
                /*  */
                /*  */

                /* MANAGE RECEIVED RESPONSE */
                /*  */

                /*  */
                /*  */
                return Ok(DEBIT_CONFIRM);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
