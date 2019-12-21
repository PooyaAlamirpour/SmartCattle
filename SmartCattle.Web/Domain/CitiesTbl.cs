using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CitiesTbl
    {
        public virtual int ID { get; set; }
        public virtual int region_id { get; set; }
        public virtual int country_id { get; set; }
        public virtual String latitude { get; set; }
        public virtual String longitude { get; set; }
        public virtual String name { get; set; }
    }

    class CitiesTblMapping : ClassMap<CitiesTbl>
    {
        public CitiesTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.region_id).Nullable();
            Map(x => x.country_id).Nullable();
            Map(x => x.latitude).Nullable();
            Map(x => x.longitude).Nullable();
            Map(x => x.name).Nullable();

            Table("SmartCattle.CitiesTbl");
        }
    }
}