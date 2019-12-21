using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    class Position
    {
        public string type { get; set; }
        public string deviceId { get; set; }
        public long detectorTime { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int spId { get; set; }
        public int packetType { get; set; }
        public int uuId { get; set; }
        public String MAC { get; set; }
    }
}
