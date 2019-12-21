using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CountriesTbl
    {
        public virtual int ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Code { get; set; }
    }

    class CountriesTblMapping : ClassMap<CountriesTbl>
    {
        public CountriesTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Name).Nullable();
            Map(x => x.Code).Nullable();

            Table("SmartCattle.CountriesTbl");
        }
    }
}