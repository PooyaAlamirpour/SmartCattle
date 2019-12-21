using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class getHTDataModel
    {
        public long detectorTime { get; set; }
        public double humidity { get; set; }
        public double temperature { get; set; }
        public string mac { get; set; }
        public int id { get; set; }
        public String Time { get; set; }
    }
}
