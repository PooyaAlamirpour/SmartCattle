using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Areas.APIs.Models
{
    public class X_CattleSensorValues
    {
        public int ID { get; set; }
        public int FarmId { get; set; }
        public int CattleId { get; set; }
        public String MacAddress { get; set; }
        public List<Temperature> CattleTemperature { get; set; }
        public List<Activity> CattleActivity { get; set; }

        public class Temperature
        {
            public String value { get; set; }
            public String Date { get; set; }
        }

        public class Activity
        {
            public double standing { get; set; }
            public double walking { get; set; }
            public double eating { get; set; }
            public double drinking { get; set; }
            public double sitting { get; set; }
            public double ruminating { get; set; }
            public String Date { get; set; }
        }
    }
}