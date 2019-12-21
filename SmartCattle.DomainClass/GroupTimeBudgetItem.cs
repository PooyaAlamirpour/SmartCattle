using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
  public  class GroupTimeBudgetItem:BaseEntity
    { 
        public TimeBudgetItem Item { get; set; }
        public double valuePercent { get; set; }
        public string colorCode { get; set; }
        public string description { get; set; }
        public int TimeBudgetId { get; set; }
        public virtual GroupTimeBudget TimeBudget { get; set; }
        public virtual SmartCattleUser User { get; set; }
        public virtual Farm Farm { get; set; }
        public int FarmID { get; set; }
    }

    public enum TimeBudgetItem
    {
        Eating,
        Lying,
        Milkimg,
        Drinking,
        Ruminating,
        Standing
    }
}
