using Newtonsoft.Json;
using Rimit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace Utility
{
    public class Content
    {
        public Result result { get; set; }
        public USERDATA data { get; set; }
    }
    
    public class Contents
    {
        public Content content { get; set; }
    }

    public class Result
    {
        public int code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }

    public class Response
    {
        public Head head { get; set; }
        public Rimit.Models.Content content { get; set; }
    }
    public class Request
    {
        public Response confirmRequest(String head, String result, String data, String uri, String key)
        {


            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            Result objCustomer = jsonSerializer.Deserialize<Result>(result);
            USERDATA user_data = jsonSerializer.Deserialize<USERDATA>(data);
            Heads _head = jsonSerializer.Deserialize<Heads>(head);

            Content content = new Content();
            content.result = objCustomer;
            content.data = user_data;

            Contents encryptData = new Contents();
            encryptData.content = content;


            var stringData = new JavaScriptSerializer().Serialize(encryptData);


            Crypto crypto = new Crypto();
            var encrypted = crypto.encryptRimitData(stringData, key);




            AddAccountModel addAccountModel = new AddAccountModel();
            addAccountModel.head = _head;
            addAccountModel.encrypted_data = encrypted;

            var requestBody = new JavaScriptSerializer().Serialize(addAccountModel);
            try
            {
                WebRequest wRequest = WebRequest.Create(uri);
                wRequest.Method = "POST";
                string postString = requestBody;
                byte[] bArray = Encoding.UTF8.GetBytes(postString);
                wRequest.ContentType = "application/json";
                wRequest.ContentLength = bArray.Length;
                Stream webData = wRequest.GetRequestStream();
                webData.Write(bArray, 0, bArray.Length);
                webData.Close();
                WebResponse webResponse = wRequest.GetResponse();
                webData = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(webData);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                webData.Close();
                webResponse.Close();
                JavaScriptSerializer jsonSerializers = new JavaScriptSerializer();
                RequestModel responce = jsonSerializers.Deserialize<RequestModel>(responseFromServer);
                var decrypted = crypto.decryptRimitData(responce.encrypted_data, key);

                AccountFetchDecrypt decrypt = jsonSerializer.Deserialize<AccountFetchDecrypt>(decrypted);


                Response responseData = new Response();
                responseData.head = responce.head;
                responseData.content = decrypt.content;
                return responseData;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                dynamic obj = JsonConvert.DeserializeObject(resp);
                var messageFromServer = obj.error.message;
                return messageFromServer;
            }

        }
    }
}