using Rimit.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using Utility;

namespace Rimit.Controllers
{
    [RoutePrefix("Api")]
    public class accountStatementController : ApiController
    {
        private Config config = new Config();

        [Route("accountStatement/{tenant_id}")]
        [HttpPost]
        public IHttpActionResult Statement([FromUri()] int tenant_id, [FromBody()] RequestModel req)
        {
            Rimit.Utility.Response res = new Rimit.Utility.Response();
            Head head = new Head();
            head.apiVersion = "accountStatement";
            head.api = "v1";
            head.timeStamp = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss 'Z'");


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


                string ENCRYPTION_KEY = ConfigurationManager.AppSettings["ENCRYPTION_KEY"];


                var REQUEST_DATA = req.encrypted_data;
                Crypto cry = new Crypto();
                var DECRYPTED_DATA = cry.decryptRimitData(REQUEST_DATA, ENCRYPTION_KEY);


                if (string.IsNullOrEmpty(DECRYPTED_DATA))
                {
                    ResponseModel result = new ResponseModel
                    {
                        code = CommonCodes.RESULT_CODE_DECRYPTION_FAILED,
                        status = CommonCodes.STATUS_ERROR,
                        message = CommonCodes.RESULT_MESSAGE_E2008,
                    };
                    Headsss headsss_ = new Headsss();
                    headsss_.HTTP_CODE = CommonCodes.HTTP_CODE_BAD_REQUEST;
                    var json = new JavaScriptSerializer().Serialize(result);


                    return BadRequest(res.error(new JavaScriptSerializer().Serialize(headsss_), new JavaScriptSerializer().Serialize(result), ""));
                };
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                AccountStatement objCustomer = jsonSerializer.Deserialize<AccountStatement>(DECRYPTED_DATA);
                /*  */
                /*  */
                /* VERIFY THE USER */
                /*
                 * MANAGE SCOPE FOR ERRORS (Refer -
                 * https://doc.rimit.co/account/account-statement#response-code)
                 */
                /*  */
                /*  */

                /*  */
                /*
                 * EG FOR FAILED RESPONSE : FIND USER ACCOUNT, IF NOT FOUND, SEND RESPONSE AS
                 * FAILED
                 */


                bool FIND_USER = true;
                if (!FIND_USER)
                {
                    ResponseModel result = new ResponseModel
                    {
                        code = CommonCodes.RESULT_CODE_INVALID_ACCOUNT,
                        status = CommonCodes.STATUS_FAILED,
                        message = CommonCodes.RESULT_MESSAGE_E2021,
                    };
                    var json = new JavaScriptSerializer().Serialize(result);
                    Headsss headssss = new Headsss();
                    headssss.HTTP_CODE = CommonCodes.HTTP_CODE_SUCCESS;
                    return Ok(res.success(new JavaScriptSerializer().Serialize(headssss), new JavaScriptSerializer().Serialize(result), "", ENCRYPTION_KEY));

                };

                /*  */

                /*  */
                /* FIND THE ACCOUNT BALANCE AND ASSIGN. KEEP 0 IF NO BALANCE FOUND*/
                var ACC_BALANCE = "0";
                /*  */

                /*  */
                /* FIND ALL TRANSACTIONS BETWEEN START_DATE & END_DATE IN THE RESPECTIVE ACCOUNT */
                List<ACCOUNT_TRANSACTION> aCCOUNT_TRANSACTIONs = new List<ACCOUNT_TRANSACTION>();   

                ACCOUNT_TRANSACTION aCCOUNT_TRANSACTION = new ACCOUNT_TRANSACTION();
                aCCOUNT_TRANSACTION.txn_id = "";
                aCCOUNT_TRANSACTION.date = "";
                aCCOUNT_TRANSACTION.time = "";
                aCCOUNT_TRANSACTION.debit_amount = "";
                aCCOUNT_TRANSACTION.credit_amount = "";
                aCCOUNT_TRANSACTION.balance = "";
                aCCOUNT_TRANSACTION.description = "";
                aCCOUNT_TRANSACTIONs.Add(aCCOUNT_TRANSACTION);
                ACCOUNT_TRANSACTION aCCOUNT_TRANSACTION1 = new ACCOUNT_TRANSACTION();
                aCCOUNT_TRANSACTION.txn_id = "";
                aCCOUNT_TRANSACTION.date = "";
                aCCOUNT_TRANSACTION.time = "";
                aCCOUNT_TRANSACTION.debit_amount = "";
                aCCOUNT_TRANSACTION.credit_amount = "";
                aCCOUNT_TRANSACTION.balance = "";
                aCCOUNT_TRANSACTION.description = "";
                aCCOUNT_TRANSACTIONs.Add(aCCOUNT_TRANSACTION1);

                USER_ACCOUNT_DATA uSER_ACCOUNT_DATA = new USER_ACCOUNT_DATA();
                uSER_ACCOUNT_DATA.branch_code = objCustomer.content.data.branch_code;
                uSER_ACCOUNT_DATA.account_number = objCustomer.content.data.account_number;
                uSER_ACCOUNT_DATA.balance_amount = ACC_BALANCE;
                uSER_ACCOUNT_DATA.start_date = objCustomer.content.data.start_date;
                uSER_ACCOUNT_DATA.end_date = objCustomer.content.data.end_date;
                uSER_ACCOUNT_DATA.transactions_count = aCCOUNT_TRANSACTIONs.Count;

                ResponseModel resulta = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,
                };
                var jsona = new JavaScriptSerializer().Serialize(resulta);
                Headsss headsss = new Headsss();
                headsss.HTTP_CODE = CommonCodes.HTTP_CODE_SUCCESS;
                DatARequesting datARequesting = new DatARequesting();
                datARequesting.account = uSER_ACCOUNT_DATA;
                datARequesting.transactions = aCCOUNT_TRANSACTIONs;

                return Ok(res.success(new JavaScriptSerializer().Serialize(headsss), new JavaScriptSerializer().Serialize(resulta), new JavaScriptSerializer().Serialize(datARequesting), ENCRYPTION_KEY));

            }
            catch (Exception)
            {

                ResponseModel resulta = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SERVICE_NOT_AVAILABLE,
                    status = CommonCodes.STATUS_ERROR,
                    message = CommonCodes.RESULT_MESSAGE_E2003,
                };
                Headsss headsss = new Headsss();
                headsss.HTTP_CODE = CommonCodes.HTTP_CODE_SERVICE_UNAVAILABLE;
                var jsona = new JavaScriptSerializer().Serialize(resulta);

                return Ok(res.error(new JavaScriptSerializer().Serialize(headsss), new JavaScriptSerializer().Serialize(resulta), ""));
            }
        }

    }
}
