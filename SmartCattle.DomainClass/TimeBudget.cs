using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class TimeBudget:BaseEntity
    { 
        public DateTime date { get; set; } 
        public ICollection<BudgetItem> TimeBudgetItems { get; set; }
        public int CattleId { get; set; }
        public virtual Cattle Cattle{ get; set; }  
    }
    public class BudgetItem
    {
        public int ID { get; set; }
        public TimeBudgetItem Item { get; set; }
        public double valuePercent { get; set; }
        public string description { get; set; }    
    }

}
