using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    class EnvSensors
    {
        public virtual int id { get; set; }
        public virtual int FreeStallId { get; set; }
        public virtual int FarmId { get; set; }
        public virtual double Lat { get; set; }
        public virtual double Lng { get; set; }
        public virtual String MAC { get; set; }
    }

    class EnvSensorsMapping : ClassMap<EnvSensors>
    {
        public EnvSensorsMapping()
        {
            Id(x => x.id);
            Map(x => x.FreeStallId).Not.Nullable();
            Map(x => x.FarmId).Not.Nullable();
            Map(x => x.Lat).Not.Nullable();
            Map(x => x.Lng).Not.Nullable();
            Map(x => x.MAC).Not.Nullable();
            Table("SmartCattle.EnvSensors");
        }
    }
}

