using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    class Activities
    {
        public string type { get; set; }
        public string MAC { get; set; }
        public long detectorTime { get; set; }
        public Activity activity { get; set; }
        public int spId { get; set; }

        public class Activity
        {
            public int standing { get; set; }
            public int walking { get; set; }
            public int eating { get; set; }
            public int drinking { get; set; }
            public int sitting { get; set; }
            public int ruminating { get; set; }
        }
    }
}
