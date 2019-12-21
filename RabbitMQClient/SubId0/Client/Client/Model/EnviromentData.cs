using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    class EnviromentData
    {
        public string MAC { get; set; }
        public long detectorTime { get; set; }
        public string dgnId { get; set; }
        public int batteryLevel { get; set; }
        public int packetType { get; set; }
        public double humidity { get; set; }
        public double temperature { get; set; }
        public int subId { get; set; }
        public string uniqueKey { get; set; }
        public string type { get; set; }
    }
}
