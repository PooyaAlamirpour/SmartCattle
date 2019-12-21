using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class getSpecActivityModel
    {
        public int _id { get; set; }
        public string MAC { get; set; }
        public string detectorTime { get; set; }
        public Activity activity { get; set; }

        public class Activity
        {
            public double standing { get; set; }
            public double walking { get; set; }
            public double eating { get; set; }
            public double drinking { get; set; }
            public double sitting { get; set; }
            public double ruminating { get; set; }
        }
    }
}