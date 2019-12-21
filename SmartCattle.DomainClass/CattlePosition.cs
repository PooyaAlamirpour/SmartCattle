using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class CattlePosition:BaseEntity
    { 
        public string value { get; set; } 
        public double x { get; set; }
        public double y { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DbGeography LatLong { get; set; }
        public int cattleId { get; set; }
        public DateTime date { set; get; }
        public long LastRecievedId { get; set; }
        public virtual Cattle Cattle { get; set; } 
        public virtual SmartCattleUser User { get; set; }
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }
    }
}
