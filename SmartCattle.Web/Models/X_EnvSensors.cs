using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models
{
    public class X_EnvSensors
    {
        public int id { get; set; }
        public int FreeStallId { get; set; }
        public int FarmId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public String MAC { get; set; }
    }
}