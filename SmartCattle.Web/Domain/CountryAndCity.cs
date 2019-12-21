using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CountryAndCity
    {
        public virtual int ID { get; set; }
        public virtual String CountryFa { get; set; }
        public virtual String CityFa { get; set; }
        public virtual String CountryEn { get; set; }
        public virtual String CityEn { get; set; }
    }

    class CountryAndCityMapping : ClassMap<CountryAndCity>
    {
        public CountryAndCityMapping()
        {
            Id(x => x.ID);
            Map(x => x.CountryFa).Nullable();
            Map(x => x.CityFa).Nullable();
            Map(x => x.CountryEn).Nullable();
            Map(x => x.CityEn).Nullable();

            Table("SmartCattle.CountryAndCity");
        }
    }
}