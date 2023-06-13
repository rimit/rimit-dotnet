using Rimit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Utility;

namespace Rimit.Utility
{
    public class Response
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

        public class Responses
        {
            public Head head { get; set; }
            public Content content { get; set; }
        }
        public String success(String head, String result, String data, String key)
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



            AddAccountModel statusResponse = new AddAccountModel();
            statusResponse.head = _head;
            statusResponse.encrypted_data = encrypted;



            return new JavaScriptSerializer().Serialize(statusResponse);
        }

        public String error(String head, String result, String data)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            Result objCustomer = jsonSerializer.Deserialize<Result>(result);
            USERDATA user_data = jsonSerializer.Deserialize<USERDATA>(data);
            Head _head = jsonSerializer.Deserialize<Head>(head);

            Content content = new Content();
            content.result = objCustomer;
            content.data = user_data;

            Responses responseData = new Responses();
            responseData.content = content;
            responseData.head = _head;




            return new JavaScriptSerializer().Serialize(responseData);
        }
    }
}