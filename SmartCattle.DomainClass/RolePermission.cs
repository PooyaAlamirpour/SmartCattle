using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class RolePermission:BaseEntity
    {
        public RolePermission()
        {
            UserRoles = new List<UserRole>();
        }
        public string Title { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public string Description { set; get; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    } 
}
