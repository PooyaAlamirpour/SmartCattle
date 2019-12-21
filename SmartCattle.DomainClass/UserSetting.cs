using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class UserSetting:BaseEntity
    { 
        public string key { get; set; }
        public string value { get; set; }
        public virtual SmartCattleUser User { get; set; }
    }
}
