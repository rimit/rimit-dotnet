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
    public class TransactionCreditController : ApiController
    {
        private Config config = new Config();

        [Route("Credit/{tenant_id}")]
        [HttpPost]
        public IHttpActionResult Credit([FromUri()] int tenant_id, [FromBody()] RequestModel req)
        {
            Rimit.Utility.Response res = new Rimit.Utility.Response();

            Head head = new Head();
            head.apiVersion = "creditAmount";
            head.api = "v1";
            head.timeStamp = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss A");
            try
            {
                string Tenent_ID = "";
                if (config.IS_MULTY_TENANT_PLATFORM == "YES")
                {
                    if (config.MULTY_TENANT_MODE == "QUERY")
                    {
                        Tenent_ID = tenant_id.ToString();
                    }
                    else if (config.MULTY_TENANT_MODE == "PARAMS")
                    {
                        Tenent_ID = tenant_id.ToString();
                    }
                }

                String AUTH_API_ID = ConfigurationManager.AppSettings["API_ID"]; ;
                String AUTH_API_KEY = ConfigurationManager.AppSettings["API_KEY"];
                /*  */

                // ADD_ACCOUNT REQUEST URL
                String DEBIT_CONFIRM_URL = config.BASE_URL + "/transaction/confirmCredit";

                string ENCRYPTION_KEY = ConfigurationManager.AppSettings["ENCRYPTION_KEY"];


                Auth auth = new Auth();
                auth.API_ID = AUTH_API_ID;
                auth.API_KEY = AUTH_API_KEY;
                Heads DEBIT_CONFIRM_HEAD = new Heads();
                DEBIT_CONFIRM_HEAD.apiVersion = "confirmDebit";
                DEBIT_CONFIRM_HEAD.api = "v1";
                DEBIT_CONFIRM_HEAD.timeStamp = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss a");
                DEBIT_CONFIRM_HEAD.auth = auth;



                var REQUEST_DATA = req.encrypted_data;
                Crypto cry = new Crypto();
                var DECRYPTED_DATA = cry.decryptRimitData(REQUEST_DATA, ENCRYPTION_KEY);


                if (string.IsNullOrEmpty(DECRYPTED_DATA))
                {
                    ResponseModel result_ = new ResponseModel
                    {
                        code = CommonCodes.RESULT_CODE_DECRYPTION_FAILED,
                        status = CommonCodes.STATUS_ERROR,
                        message = CommonCodes.RESULT_MESSAGE_E2008,
                    };
                    Headsss headsss_ = new Headsss();
                    headsss_.HTTP_CODE = CommonCodes.HTTP_CODE_BAD_REQUEST;
                    var json = new JavaScriptSerializer().Serialize(result_);


                    return BadRequest(res.error(new JavaScriptSerializer().Serialize(headsss_), new JavaScriptSerializer().Serialize(result_), ""));
                };

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                DebitModel objCustomer = jsonSerializer.Deserialize<DebitModel>(DECRYPTED_DATA);
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

                var USER = objCustomer.content.data.user;
                var TRANSACTION = objCustomer.content.data.transaction;
                var SETTLEMENT = objCustomer.content.data.settlement;
                DEBIT_CONFIRM_DATA dEBIT_CONFIRM_DATA = new DEBIT_CONFIRM_DATA();
                dEBIT_CONFIRM_DATA.txn_urn = TRANSACTION.txn_urn;
                dEBIT_CONFIRM_DATA.txn_number = TRANSACTION.txn_number;
                dEBIT_CONFIRM_DATA.TRANSACTION_REF = TRANSACTION_REF;
                dEBIT_CONFIRM_DATA.txn_amount = TRANSACTION.txn_amount;
                dEBIT_CONFIRM_DATA.txn_type = TRANSACTION.txn_type;
                dEBIT_CONFIRM_DATA.txn_nature = TRANSACTION.txn_nature;
                dEBIT_CONFIRM_DATA.account_balance = ACCOUNT_BALANCE;


                /*  */
                /* EG FOR FAILED REQUEST : FIND LATEST ACCOUNT BALANCE, IF FOUND INSUFFICIENT, SEND REQUEST AS FAILED */


                bool CHECK_LATEST_BALANCE = true;

                var DEBIT_CONFIRM_Data = new JavaScriptSerializer().Serialize(dEBIT_CONFIRM_DATA);
                var _head = new JavaScriptSerializer().Serialize(DEBIT_CONFIRM_HEAD);
                if (!CHECK_LATEST_BALANCE)
                {
                    ResponseModel results = new ResponseModel
                    {
                        code = CommonCodes.RESULT_CODE_INSUFFICIENT_ACCOUNT_BALANCE,
                        status = CommonCodes.STATUS_ERROR,
                        message = CommonCodes.RESULT_MESSAGE_E8897,
                    };
                    var DEBIT_CONFIRM_RESULT = new JavaScriptSerializer().Serialize(results);


                    Request request_ = new Request();
                    var ADD_ACCOUNT_CONFIRM_ = request_.confirmRequest(_head, DEBIT_CONFIRM_RESULT, DEBIT_CONFIRM_Data, DEBIT_CONFIRM_URL, ENCRYPTION_KEY);
                    return BadRequest();
                }
                ResponseModel result = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,
                };
                var _DEBIT_CONFIRM_RESULT = new JavaScriptSerializer().Serialize(result);
                Request request = new Request();
                var ADD_ACCOUNT_CONFIRM = request.confirmRequest(_head, _DEBIT_CONFIRM_RESULT, DEBIT_CONFIRM_Data, DEBIT_CONFIRM_URL, ENCRYPTION_KEY);
                /*  */
                /*  */

                /* MANAGE RECEIVED RESPONSE */
                /*  */

                /*  */
                /*  */
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
