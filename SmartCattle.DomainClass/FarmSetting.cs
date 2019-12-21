using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class FarmSetting:BaseEntity
    { 
            public string key { get; set; }
            public string value { get; set; }
            public virtual SmartCattleUser User { get; set; }
            public int FarmID { get; set; }
            public virtual Farm Farm { get; set; }
        
    }
}
