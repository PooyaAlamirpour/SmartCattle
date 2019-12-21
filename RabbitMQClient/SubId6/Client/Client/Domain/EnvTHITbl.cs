using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    class EnvTHITbl
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
        public virtual String MAC { get; set; }
        public virtual DateTime date { get; set; }
    }

    class EnvTHITblMapping : ClassMap<EnvTHITbl>
    {
        public EnvTHITblMapping()
        {
            Id(x => x.ID);
            Map(x => x.FarmID).Not.Nullable();
            Map(x => x.FreeStallId).Not.Nullable();
            Map(x => x.TdbValue).Not.Nullable();
            Map(x => x.RHValue).Not.Nullable();
            Map(x => x.THIValue).Not.Nullable();
            Map(x => x.SensorLat).Not.Nullable();
            Map(x => x.SensorLng).Not.Nullable();
            Map(x => x.LastId).Not.Nullable();
            Map(x => x.MAC).Not.Nullable();
            Map(x => x.date).Not.Nullable();

            Table("SmartCattle.EnvTHITbl");
        }
    }
}
