using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class CattleGroup : BaseEntity
    {
        public string name { get; set; } 
        public string Description { get; set; }
        public string order { get; set; }
        public string code { get; set; }
        public virtual ICollection<Cattle> Cattles { set; get; }
        public virtual SmartCattleUser User { get; set; }
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }
    }
}
