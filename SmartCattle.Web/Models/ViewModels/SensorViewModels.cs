
using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class SensorViewModels
    {
        public int? sensorId { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Invalid MAC Address")]
        public string macAddress { get; set; } 
        public DateTime lastTransmitDate { get; set; }
        public DateTime activationDate { get; set; }
        public DateTime linkingDate { get; set; }
        public DateTime lastSyncDate { get; set; } 
        public string antennaName { get; set; }
        public string status { get; set; }
        public string softwareVersion { get; set; }
        public virtual Cattle Cattle { get; set; }
    }
}