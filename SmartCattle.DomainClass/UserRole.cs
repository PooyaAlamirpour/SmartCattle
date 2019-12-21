using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class UserRole:IdentityRole
    {
        public UserRole() : base() {

            RolePermissions = new List<RolePermission>();
        }
        public UserRole(string roleName) : base(roleName) {
            RolePermissions = new List<RolePermission>();
        }
        public string Description { set; get; } 
        public virtual SmartCattleUser User { get; set; }
        public int? FarmID { get; set; }
        public virtual Farm Farm { get; set; } 
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
