using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.Core
{
   public class Setting
    {
        public static readonly string IPARS_URL = "http://79.175.133.194:2222/";
        public static readonly string GetPositionUrl = IPARS_URL +"getPData";
        public static readonly string GetActivityUrl = IPARS_URL+"getAData";
        public static readonly string GetTemperatureUrl = IPARS_URL+"getTData";

        public static readonly string GetZoneMap = "http://192.168.101.112:2222/getZoneMap";
        public static readonly string getZoneData = "http://192.168.101.112:2222/getZoneData";


        public static readonly string SignUpUrl = IPARS_URL+"signUp";
        public static readonly string SignInUrl = IPARS_URL+"signIn";
        public static readonly string GetApiKeyUrl = IPARS_URL+"getApiKey";
        public static readonly string AdminEmail = "smart.cattle.ir@gmail.com";
        public static readonly string IPARSEmail = "info@smartcattle.ir";
        public static readonly string IPARSPassword = "H0se!n1234";
        public static readonly string IPARSPhoneNumber = "09121234567";
        public static readonly string IPARSProjectName = "SmartCattle";
 
        public static readonly int IPSGetDataIntervalSecond = 10;
    }
}
