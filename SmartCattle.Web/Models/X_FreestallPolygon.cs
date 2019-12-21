using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models
{
    public class X_FreestallPolygon
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }

        public class Geometry
        {
            public string type { get; set; }
            public List<List<List<double>>> coordinates { get; set; }
        }

        public class StyleMapHash
        {
            public string normal { get; set; }
            public string highlight { get; set; }
        }

        public class Properties
        {
            public string name { get; set; }
            public string styleUrl { get; set; }
            public string styleHash { get; set; }
            public StyleMapHash styleMapHash { get; set; }
            public string stroke { get; set; }
            public int stroke_opacity { get; set; }
            public double stroke_width { get; set; }
            public string fill { get; set; }
            public object fill_opacity { get; set; }
        }
    }
}