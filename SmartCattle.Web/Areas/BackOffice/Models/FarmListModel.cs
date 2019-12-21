using System;
using System.Collections;
using System.Collections.Generic;
using SmartCattle.DomainClass;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Areas.BackOffice.Models
{
    public class FarmListModel : BaseEntity, IEnumerable
    {
        public List<Farm> Farms { get; set; }
        public IQueryable<IGrouping<int, Sensor>> FarmSensors { get; set; }
        //========================================
        //public string FarmName { get; set; }
        //public int SensorCount { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}