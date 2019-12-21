using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class CattleScore:BaseEntity
    { 
        public ScoreTypes item { get; set; }
        public double value { get; set; }
        public int CattleId { get; set; }
        public String UserName { get; set; }
        public DateTime Date { get; set; }
        public virtual Cattle Cattle { get; set; }
        public int UserIdentity { get; set; }
    }

    public enum ScoreTypes
    {
        Body_Condition_Score,
        Cleanliness,
        Hock,
        Mobility,
        Manure,
        Rumen,
        Teat,
        Milk_Production
    }
}
