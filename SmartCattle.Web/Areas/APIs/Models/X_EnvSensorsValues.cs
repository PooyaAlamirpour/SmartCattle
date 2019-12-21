using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Areas.APIs.Models
{
    public class X_EnvSensorsValues
    {
        public int ID { get; set; }
        public int FarmId { get; set; }
        public int FreeStallId { get; set; }
        public String MacAddress { get; set; }
        public double SensorLat { get; set; }
        public double SensorLng { get; set; }
        public List<ValuesClass> Values { get; set; }

        public class ValuesClass
        {
            public String Temperature { get; set; }
            public String Humidity { get; set; }
            public String THI { get; set; }
            public String Date { get; set; }
        }
    }
}