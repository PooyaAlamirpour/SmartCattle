using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Helper
{
    public static class Localization
    {
        /// <summary>
        /// get equivalent expression in Resources
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getString(string key)
        {
            if(key == null)
            {
                return "NULL";
            }
            else
            {
                string val = App_LocalResources.Resource.ResourceManager.GetString(key);
                if (!string.IsNullOrEmpty(val))
                {
                    return val;
                }
                else
                {
                    return key;
                }
            }
            
        }
    }
}