using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class EnvTHITbl
    {
        public virtual int ID { get; set; }
        public virtual int FarmID { get; set; }
        public virtual int FreeStallId { get; set; }
        public virtual double TdbValue { get; set; }
        public virtual double RHValue { get; set; }
        public virtual double THIValue { get; set; }
        public virtual double SensorLat { get; set; }
        public virtual double SensorLng { get; set; }
        public virtual int LastId { get; set; }
        public virtual string MAC { get; set; }
        public virtual DateTime date { get; set; }
    }

    class EnvTHITblMapping : ClassMap<EnvTHITbl>
    {
        public EnvTHITblMapping()
        {
            Id(x => x.ID);
            Map(x => x.FarmID).Nullable();
            Map(x => x.FreeStallId).Nullable();
            Map(x => x.TdbValue).Nullable();
            Map(x => x.RHValue).Nullable();
            Map(x => x.THIValue).Nullable();
            Map(x => x.SensorLat).Nullable();
            Map(x => x.SensorLng).Nullable();
            Map(x => x.LastId).Nullable();
            Map(x => x.MAC).Nullable();
            Map(x => x.date).Nullable();

            Table("SmartCattle.EnvTHITbl");
        }
    }
}