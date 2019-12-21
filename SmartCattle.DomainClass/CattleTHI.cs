using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class CattleTHI:BaseEntity
    { 
        public DateTime date { get; set; }
        public double TdbValue { get; set; }
        public double RHValue { get; set; }
        public double THIValue { get; set; }
        public DbGeography Location { get; set; }
        public int? FreeStallId { get; set; }
        public virtual SmartCattleUser User { get; set; }
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }
    }
}
