using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    class BodyTemperature
    {
        public string type { get; set; }
        public int packetType { get; set; }
        public string MAC { get; set; }
        public long detectorTime { get; set; }
        public double tObj { get; set; }
        public double tAmb { get; set; }
        public int spId { get; set; }
    }
}
