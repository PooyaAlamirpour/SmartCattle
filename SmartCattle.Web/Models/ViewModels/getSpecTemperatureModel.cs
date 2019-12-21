using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class getSpecTemperatureModel
    {
        public int _id { get; set; }
        public string MAC { get; set; }
        public string detectorTime { get; set; }
        public double tObj { get; set; }
    }
}