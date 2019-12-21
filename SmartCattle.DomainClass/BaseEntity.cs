using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public abstract class BaseEntity
    {
        public string UserId { get; set; }       
        public int ID { get; set; }
        //public int index { get; set; }
    }
}
