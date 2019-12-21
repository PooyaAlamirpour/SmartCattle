using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
  public class CustomUseRole:IdentityUserRole<string>
    {

        public int ID { get; set; }
        public CustomUseRole():base()
        {
        }
        
        public override string RoleId
        {
            get;
            set;
        }
        public override string UserId
        {
            get;
            set;
        }
        
    }
}
