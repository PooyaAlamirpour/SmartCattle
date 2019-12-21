using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.Core.DTO
{
  public class CattleTHIDTO
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public double TdbValue { get; set; }
        public double RHValue { get; set; }
        public int CattleID { get; set; }
        public int userId { get; set; }
    }
}
