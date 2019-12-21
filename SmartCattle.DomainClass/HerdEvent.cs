using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class HerdEvent : BaseEntity
    { 
        public string title { get; set; }
        public string description { get; set; }
        public DateTime eventDate { get; set; }
        public int herdId { get; set; }
        public virtual CattleHerd Herd { get; set; }
        public virtual SmartCattleUser User { get; set; }
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }
    }
}
