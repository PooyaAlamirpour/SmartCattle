using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
    public class GroupTimeBudget:BaseEntity
    { 
        public seasons season { get; set; }
        public string Title { get; set; }
        public DateTime date { get; set; }
        public int FarmID { get; set; }
        public int CattleGroupId { get; set; }
        public virtual ICollection<GroupTimeBudgetItem> Items { get; set; }
        public virtual SmartCattleUser User { get; set; }
        public virtual CattleGroup CattleGroup { get; set; }
        public virtual Farm Farm { get; set; }
    }
    public enum seasons
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
}
