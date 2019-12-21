using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
    public class ApplicationSetting : BaseEntity
    { 
        public string key { get; set; }
        public string value { get; set; } 

    }
}
