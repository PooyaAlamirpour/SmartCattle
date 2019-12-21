using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class CattleTempreture : BaseEntity
    { 
        public double value { get; set; } 
        public string point { get; set; }
        public int cattleId { get; set; }
        public DateTime date { set; get; }
        public long LastRecievedId { get; set; }
        public virtual Cattle Cattle { get; set; }
        public virtual SmartCattleUser User { get; set; }
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }
    }
}
