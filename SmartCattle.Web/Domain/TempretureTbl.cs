﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class TempretureTbl
    {
        public virtual int ID { get; set; }
        public virtual double value { get; set; }
        public virtual int cattleId { get; set; }
        public virtual DateTime date { get; set; }
        public virtual int FarmID { get; set; }
        public virtual int FreeStall { get; set; }
        public virtual String dateStr { get; set; }
    }

    class TempretureTblMapping : ClassMap<TempretureTbl>
    {
        public TempretureTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.value).Not.Nullable();
            Map(x => x.cattleId).Not.Nullable();
            Map(x => x.date).Not.Nullable();
            Map(x => x.FarmID).Not.Nullable();
            Map(x => x.FreeStall).Nullable();
            Map(x => x.dateStr).Nullable();

            Table("SmartCattle.TempretureTbl");
        }
    }
}