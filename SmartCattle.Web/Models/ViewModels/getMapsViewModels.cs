using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class getMapsViewModels
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
        public Center center { get; set; }

        public class Geometry
        {
            public string type { get; set; }
            public List<List<object>> coordinates { get; set; }
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
            public int _invalid_name_stroke_opacity { get; set; }
            public double _invalid_name_stroke_width { get; set; }
            public string fill { get; set; }
            public double? _invalid_name_fill_opacity { get; set; }
        }

        public class Feature
        {
            public string type { get; set; }
            public Geometry geometry { get; set; }
            public Properties properties { get; set; }
        }

        public class Center
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }
    }
}