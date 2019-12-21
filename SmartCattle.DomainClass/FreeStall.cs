using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
    public class FreeStall : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual String name { get; set; }
        public virtual String Description { get; set; }
        public virtual String ServerName { get; set; }
        public virtual int FarmID { get; set; }
        public virtual int GroupID { get; set; }
        public virtual String UserId { get; set; }
    }
}
