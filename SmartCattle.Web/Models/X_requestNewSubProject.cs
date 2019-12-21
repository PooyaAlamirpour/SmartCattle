using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models
{
    public class X_requestNewSubProject
    {
        public int statusCode { get; set; }
        public Message message { get; set; }

        public class Message
        {
            public int spId { get; set; }
            public string dataQueue { get; set; }
            public string EquipmentQueue { get; set; }
        }
    }
}