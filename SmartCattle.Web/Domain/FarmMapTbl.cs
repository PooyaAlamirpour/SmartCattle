using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class FarmMapTbl
    {
        public virtual int ID { get; set; }
        public virtual String Map { get; set; }
        public virtual int SubId { get; set; }
        public virtual int FarmId { get; set; }
        public virtual DateTime CreateDate { get; set; }
    }

    class FarmMapTblMapping : ClassMap<FarmMapTbl>
    {
        public FarmMapTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Map).Nullable();
            Map(x => x.SubId).Nullable();
            Map(x => x.FarmId).Nullable();
            Map(x => x.CreateDate).Nullable();

            Table("SmartCattle.FarmMapTbl");
        }
    }
}