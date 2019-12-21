using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Areas.APIs.Models
{
    public class X_CattleData
    {
        public int FarmId { get; set; }
        public int CattleId { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public int Step { get; set; }
    }
}