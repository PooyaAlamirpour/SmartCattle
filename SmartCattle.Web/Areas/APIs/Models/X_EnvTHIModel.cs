using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Areas.APIs.Models
{
    public class X_EnvTHIModel
    {
        public long detectorTime { get; set; }
        public double humidity { get; set; }
        public double temperature { get; set; }
        public double THI { get; set; }
        public string mac { get; set; }
        public int _id { get; set; }
        public String Time { get; set; }
    }
}