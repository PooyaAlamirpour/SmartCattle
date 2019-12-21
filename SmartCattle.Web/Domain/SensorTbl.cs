using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class SensorTbl
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
            Map(x => x.MacAddress).Nullable();
            Map(x => x.cattleId).Nullable();
            Map(x => x.FarmID).Nullable();

            Table("SmartCattle.SensorTbl");
        }
    }
}