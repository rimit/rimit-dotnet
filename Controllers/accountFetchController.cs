using Newtonsoft.Json;
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
    public class accountFetchController : ApiController
    {
        private Config config = new Config();

        [Route("AccountFetch/{tenant_id}")]
        [HttpPost]
        public IHttpActionResult AccountFetch([FromUri()] int tenant_id, [FromBody()] RequestModel req)
        {
            Rimit.Utility.Response res = new Rimit.Utility.Response();

            Head head = new Head();
            head.apiVersion = "accountFetch";
            head.api = "v1";
            head.timeStamp = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss a");
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
                    var json = new JavaScriptSerializer().Serialize(result);
                    Headsss head_ = new Headsss();
                    head_.HTTP_CODE = CommonCodes.HTTP_CODE_BAD_REQUEST;

                    return BadRequest(res.error(new JavaScriptSerializer().Serialize(head_), new JavaScriptSerializer().Serialize(result),""));
                };
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                AccountFetchDecrypt objCustomer = jsonSerializer.Deserialize<AccountFetchDecrypt>(DECRYPTED_DATA);
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
                        code = CommonCodes.RESULT_CODE_MOBILE_NUMBER_NOT_FOUND,
                        status = CommonCodes.STATUS_FAILED,
                        message = CommonCodes.RESULT_MESSAGE_E2014,
                    };
                    var json = new JavaScriptSerializer().Serialize(result);
                    Headsss headssss = new Headsss();
                    headssss.HTTP_CODE = CommonCodes.HTTP_CODE_SUCCESS;
                    return Ok(res.success(new JavaScriptSerializer().Serialize(headssss), new JavaScriptSerializer().Serialize(result), "",ENCRYPTION_KEY));
                };


                USERDATA USER_DATA = new USERDATA();
                USER_DATA.country_code = objCustomer.content.data.country_code;
                USER_DATA.mobile = objCustomer.content.data.mobile;
                Addaccount(USER_DATA);

                ResponseModel resulta = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,
                };
                var jsona = new JavaScriptSerializer().Serialize(resulta);
                Headsss headsss = new Headsss();
                headsss.HTTP_CODE = CommonCodes.HTTP_CODE_SUCCESS;
                return Ok(res.success(new JavaScriptSerializer().Serialize(headsss), new JavaScriptSerializer().Serialize(resulta), "", ENCRYPTION_KEY));

            }
            catch (Exception)
            {
               
                ResponseModel resulta = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SERVICE_NOT_AVAILABLE,
                    status = CommonCodes.STATUS_ERROR,
                    message = CommonCodes.RESULT_MESSAGE_E2003,
                };
                var jsona = new JavaScriptSerializer().Serialize(resulta);

                return Ok(jsona);
            }
}



        private bool Addaccount(USERDATA user)
        {
            try
            {
                String AUTH_API_ID = ConfigurationManager.AppSettings["API_ID"]; ;
                String AUTH_API_KEY = ConfigurationManager.AppSettings["API_KEY"];
                /*  */

                // ADD_ACCOUNT REQUEST URL
                String ADD_ACCOUNT_URL = config.BASE_URL + "/account/add";

                string ENCRYPTION_KEY = ConfigurationManager.AppSettings["ENCRYPTION_KEY"];
                /*  */
                /* ASSIGN USER DATA BASED ON REQUEST DATA ON accountFetch */

                Auth auth = new Auth();
                auth.API_ID = AUTH_API_ID;
                auth.API_KEY = AUTH_API_KEY;
                Heads ADD_ACCOUNT_HEAD = new Heads();
                ADD_ACCOUNT_HEAD.apiVersion = "accountAdd";
                ADD_ACCOUNT_HEAD.api = "v1";
                ADD_ACCOUNT_HEAD.timeStamp = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss a");
                ADD_ACCOUNT_HEAD.auth = auth;

                /*  */

                /*  */
                /* READ ALL ACCOUNTS OF THE USER IN ACCOUNTS DATA */
                List<Account> accountList = new List<Account>();
                Account account = new Account();
                account.branch_code = "";
                account.account_number = "";
                account.account_status = "";
                account.account_class = "";
                account.account_currency = "";
                account.account_daily_limit = "";
                account.account_name = "";
                account.account_number = "";
                account.account_opening_date = "";
                account.account_type = "";
                account.auth_salt = "";
                account.branch_name = "";
                account.is_cash_credit_allowed = true;
                account.is_cash_debit_allowed = true;
                account.is_debit_allowed = true;
                account.is_credit_allowed = true;
                accountList.Add(account);

                Datas ADD_ACCOUNTS_DATA = new Datas();
                ADD_ACCOUNTS_DATA.accounts = accountList;
                ADD_ACCOUNTS_DATA.user = user;



                ResponseModel result = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,
                };
                var head = new JavaScriptSerializer().Serialize(ADD_ACCOUNT_HEAD);
                var results = new JavaScriptSerializer().Serialize(result);
                var _DATA = new JavaScriptSerializer().Serialize(ADD_ACCOUNTS_DATA);
                Request request = new Request();
                var ADD_ACCOUNT_CONFIRM = request.confirmRequest(head, results, _DATA, ADD_ACCOUNT_URL, ENCRYPTION_KEY);
                /*  */
                /*  */

                /* MANAGE RECEIVED RESPONSE */
                /*  */

                /*  */
                /*  */

                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        [Route("AddAccount")]
        [HttpPost]
        public IHttpActionResult AddAccount([FromBody()] USERDATA req)
        {
            try
            {
                String AUTH_API_ID = ConfigurationManager.AppSettings["API_ID"]; ;
                String AUTH_API_KEY = ConfigurationManager.AppSettings["API_KEY"];
                /*  */

                // ADD_ACCOUNT REQUEST URL
                String ADD_ACCOUNT_URL = config.BASE_URL + "/account/add";

                string ENCRYPTION_KEY = ConfigurationManager.AppSettings["ENCRYPTION_KEY"];
                /*  */
                /* ASSIGN USER DATA BASED ON REQUEST DATA ON accountFetch */

                Auth auth = new Auth();
                auth.API_ID = AUTH_API_ID;
                auth.API_KEY = AUTH_API_KEY;
                Heads ADD_ACCOUNT_HEAD = new Heads();
                ADD_ACCOUNT_HEAD.apiVersion = "accountAdd";
                ADD_ACCOUNT_HEAD.api = "v1";
                ADD_ACCOUNT_HEAD.timeStamp = DateTime.Now.ToString("YYYY-MM-dd hh:mm:ss a");
                ADD_ACCOUNT_HEAD.auth = auth;

                /*  */

                /*  */
                /* READ ALL ACCOUNTS OF THE USER IN ACCOUNTS DATA */
                List<Account> accountList = new List<Account>();
                Account account = new Account();
                account.branch_code = "";
                account.account_number = "";
                account.account_status = "";
                account.account_class = "";
                account.account_currency = "";
                account.account_daily_limit = "";
                account.account_name = "";
                account.account_number = "";
                account.account_opening_date = "";
                account.account_type = "";
                account.auth_salt = "";
                account.branch_name = "";
                account.is_cash_credit_allowed = true;
                account.is_cash_debit_allowed = true;
                account.is_debit_allowed = true;
                account.is_credit_allowed = true;
                accountList.Add(account);

                Datas ADD_ACCOUNTS_DATA = new Datas();
                ADD_ACCOUNTS_DATA.accounts = accountList;
                ADD_ACCOUNTS_DATA.user = req;



                ResponseModel result = new ResponseModel
                {
                    code = CommonCodes.RESULT_CODE_SUCCESS,
                    status = CommonCodes.STATUS_SUCCESS,
                    message = CommonCodes.RESULT_MESSAGE_E1001,
                };
                var head = new JavaScriptSerializer().Serialize(ADD_ACCOUNT_HEAD);
                var results = new JavaScriptSerializer().Serialize(result);
                var _DATA = new JavaScriptSerializer().Serialize(ADD_ACCOUNTS_DATA);
                Request request = new Request();
                var ADD_ACCOUNT_CONFIRM = request.confirmRequest(head, results, _DATA, ADD_ACCOUNT_URL, ENCRYPTION_KEY);
                /*  */
                /*  */

                /* MANAGE RECEIVED RESPONSE */
                /*  */

                /*  */
                /*  */
                return Ok(true);

            }
            catch (Exception)
            {

                return InternalServerError();
            }
        }

    }
}
