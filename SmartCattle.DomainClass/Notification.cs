using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class Notification:BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; } 
        public RuleTypes Rule { get; set; }
        public int FarmId { get; set; }
        public virtual Farm Farm { get; set; }    
    }

    public class UserNotification: BaseEntity
    {
        public DateTime Date { get; set; }
        public bool Seen { get; set; }
        public bool Received { get; set; }
        public DateTime SeenDate { get; set; } 
        public int NotificationId { get; set; }
        public int priority { get; set; }
        public string AdditionalInfo { get; set; }
        public virtual Notification Notification { get; set; }
        public virtual SmartCattleUser User { get; set; }
    }

    public class RoleNotification: BaseEntity
    {
        public int NotificationId { get; set; }
        public string RoleId { get; set; }
        public int FarmId { get; set; }
        public int priority { get; set; }
        public bool Maskable { get; set; }
        public virtual Notification Notification { get; set; }
        public virtual UserRole Role { get; set; }
        public virtual Farm Farm { get; set; }
    }
     
    public enum RuleTypes
    {
        THI,
        Heat,
        Temperature,
        Activity
    }
    
}
