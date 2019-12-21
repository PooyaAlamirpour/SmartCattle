using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class WeatherTbl
    {
        public virtual int ID { get; set; }
        public virtual String WeatherCode { get; set; }
        public virtual String Name { get; set; }
    }

    class WeatherTblMapping : ClassMap<WeatherTbl>
    {
        public WeatherTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.WeatherCode).Nullable();
            Map(x => x.Name).Nullable();

            Table("SmartCattle.WeatherTbl");
        }
    }
}