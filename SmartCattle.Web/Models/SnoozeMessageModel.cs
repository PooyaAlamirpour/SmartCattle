using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models
{
    public class SnoozeMessageModel
    {
        public List<String> SnoozeMessage { get; set; }
        public List<String> SnoozeDate { get; set; }
        public List<String> Username { get; set; }
    }
}