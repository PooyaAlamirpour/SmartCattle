using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    class SensorTbl
    {
        public virtual int ID { get; set; }
        public virtual String MacAddress { get; set; }
        public virtual int cattleId { get; set; }
        public virtual int FarmID { get; set; }
    }

    class SensorTblMapping : ClassMap<SensorTbl>
    {
        public SensorTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.MacAddress).Not.Nullable();
            Map(x => x.cattleId).Not.Nullable();
            Map(x => x.FarmID).Not.Nullable();
            Table("SmartCattle.SensorTbl");
        }
    }
}
