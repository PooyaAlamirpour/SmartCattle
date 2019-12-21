using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.Core
{
     
        public class ActionMessage
        {
            public string title { get; set; }
            public string content { get; set; }
            public object value { get; set; }
            public messageType type { set; get; }
        }

        public enum messageType
        {
            success,
            warnning,
            alert,
            info,
            error
        }
    
}
