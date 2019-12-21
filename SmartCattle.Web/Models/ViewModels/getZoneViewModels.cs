using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class getZoneViewModels
    {
        public int _id { get; set; }
        public long detectorTime { get; set; }
        public string MAC { get; set; }
        public int zoneId { get; set; }
    }
}