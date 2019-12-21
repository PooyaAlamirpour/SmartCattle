using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class RegionsTbl
    {
        public virtual int ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Code { get; set; }
        public virtual int CountryId { get; set; }
    }

    class RegionsTblMapping : ClassMap<RegionsTbl>
    {
        public RegionsTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Name).Nullable();
            Map(x => x.Code).Nullable();
            Map(x => x.CountryId).Nullable();

            Table("SmartCattle.RegionsTbl");
        }
    }
}