using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models
{
    public class CattleExcel
    {
        public int Body { get; set; }
        public double AvgMilk { get; set; }
        public int DIM { get; set; }
        public int Preg { get; set; }
        public object Group { get; set; }
        public int Lactation { get; set; }
        public string Status { get; set; }
        public string InseminationStatus { get; set; } // modiran has two cattle status
        public string BirthDate { get; set; }
        public string CalveDate { get; set; }
        public int InsCount { get; set; }
        public string InsemDate { get; set; }
    }
}