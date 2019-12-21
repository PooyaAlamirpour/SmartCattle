using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class Sensor : BaseEntity
    { 
        public string MacAddress { get; set; }
        public int? cattleId { get; set; }
        public DateTime lastTransmitDate { get; set; }
        public DateTime activationDate { get; set; }
        public DateTime linkingDate { get; set; }
        public DateTime lastSyncDate { get; set; }
        public int antennaId { get; set; }
        public string antennaName { get; set; }
        public SensorStatus status { get; set; }
        public string softwareVersion { get; set; }       
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }
    }

    public enum SensorStatus
    {
        Linked,
        NotLinked,
        Active,
        Inactive
    }
}
