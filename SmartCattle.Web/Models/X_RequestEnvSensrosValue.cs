using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models
{
    public class X_RequestEnvSensrosValue
    {
        public int FarmId { get; set; }
        public int FreeStallId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<MAC> MACs { get; set; }

        public class MAC
        {
            public string Address { get; set; }
            public int Step { get; set; }
        }
    }
}