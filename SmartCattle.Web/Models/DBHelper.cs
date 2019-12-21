using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models
{
    public class DBHelper
    {
        public static ConnectionStringSettings ConnectionStrings()
        {
            Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/SmartCattle.Web");
            ConnectionStringSettings connString = null;
            //"data source=79.175.133.194;User ID=hossein;Password=myzYKEGuP70V_oWb30Yr; initial catalog=SmartCattle"
            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connString =
                    rootWebConfig.ConnectionStrings.ConnectionStrings["SmartCattle"];
                //if (connString != null)
            }
            return connString;
        }
    }
}