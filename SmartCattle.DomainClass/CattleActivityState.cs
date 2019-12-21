using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
  public class CattleActivityState:BaseEntity
    {         
        public string jsonedActivities { get; set; }
        public decimal Sitting { get; set; }
        public decimal Standing { get; set; }
        public decimal Walking { get; set; }
        public decimal Eating { get; set; }
        public decimal Rumination { get; set; }
        public decimal Drinking { get; set; }
        public int cattleId { get; set; }
        public DateTime date { set; get; }
        public virtual Cattle Cattle { get; set; } 
        public int FarmID { get; set; }
        public long LastRecievedId { get; set; } // keep last activity Id that received from IPARS
        public virtual Farm Farm { get; set; }
    }
}
