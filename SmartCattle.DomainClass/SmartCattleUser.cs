using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class SmartCattleUser : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string NameFamily { get; set; }
        public DateTime? lastLogin { get; set; }
        public string avatarUrl { get; set; }
        public string phone { get; set; }
        public int FarmID { get; set; }
        public virtual Farm Farm { get; set; }
        public virtual ICollection<UserNotification> Notifications { get; set; }


    }
}
