using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Utility
{
    public class Config
    {
        public  string BASE_URL = ConfigurationManager.AppSettings["BASE_URL"];
        public  string IS_MULTY_TENANT_PLATFORM = ConfigurationManager.AppSettings["IS_MULTY_TENANT_PLATFORM"];
        public  string MULTY_TENANT_MODE = ConfigurationManager.AppSettings["MULTY_TENANT_MODE"];
    }
}