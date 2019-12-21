using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
    public class CattleMilking : BaseEntity
    { 
        public DateTime date { get; set; }
        public double protein { get; set; }
        public double fat { get; set; }
        public int turn { get; set; }
        public double value { get; set; }
        public double FatProteinIndex { get; set; }
        public int cattleId { get; set; }
        public double SCC { get; set; }
        public virtual Cattle Cattle { get; set; }
        public virtual SmartCattleUser User { get; set; }
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }

    }
}
