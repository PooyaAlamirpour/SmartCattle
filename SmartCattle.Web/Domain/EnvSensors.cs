using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class EnvSensors
    {
        public virtual int id { get; set; }
        public virtual int FreeStallId { get; set; }
        public virtual int FarmId { get; set; }
        public virtual double Lat { get; set; }
        public virtual double Lng { get; set; }
        public virtual string MAC { get; set; }
    }

    class EnvSensorsMapping : ClassMap<EnvSensors>
    {
        public EnvSensorsMapping()
        {
            Id(x => x.id);
            Map(x => x.FreeStallId).Nullable();
            Map(x => x.FarmId).Nullable();
            Map(x => x.Lat).Nullable();
            Map(x => x.Lng).Nullable();
            Map(x => x.MAC).Nullable();

            Table("SmartCattle.EnvSensors");
        }
    }
}