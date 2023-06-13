using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rimit.Models
{
    public class EncryptedData
    {
        public string cipher_text { get; set; }
        public string iv { get; set; }
        public string hash { get; set; }
    }
    public class Content
    {
        public Data data { get; set; }
    }
    public class Data
    {
        public string country_code { get; set; }
        public string mobile { get; set; }
        public string dob { get; set; }
    }

    public class USERDATA
    {
        public string country_code { get; set; }
        public string mobile { get; set; }
    }
    public class Head
    {
        public string api { get; set; }
        public string apiVersion { get; set; }
        public string timeStamp { get; set; }
    }

    public class RequestModel
    {
        public Head head { get; set; }
        public EncryptedData encrypted_data { get; set; }
    }

    public class AccountFetchDecrypt
    {
        public Head head { get; set; }
        public Content content { get; set; }
    }


    public class ResponseModel
    {
        public int code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
}