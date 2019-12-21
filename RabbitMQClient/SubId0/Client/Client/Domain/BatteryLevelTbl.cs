using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    public class BatteryLevelTbl
    {
        public virtual int ID { get; set; }
        public virtual String MacAddress { get; set; }
        public virtual int BatteryLevel { get; set; }
        public virtual DateTime Date { get; set; }
    }

    class BatteryLevelTblMapping : ClassMap<BatteryLevelTbl>
    {
        public BatteryLevelTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.MacAddress).Nullable();
            Map(x => x.BatteryLevel).Nullable();
            Map(x => x.Date).Nullable();

            Table("SmartCattle.BatteryLevelTbl");
        }
    }
}
