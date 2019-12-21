using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattleGroupTbl
    {
        public virtual int ID { get; set; }
        public virtual String name { get; set; }
        public virtual String Description { get; set; }
        public virtual String code { get; set; }
        public virtual DateTime date { get; set; }
        public virtual int FarmID { get; set; }
        public virtual String UserName { get; set; }
        public virtual int UserIdentity { get; set; }
    }

    class CattleGroupTblMapping : ClassMap<CattleGroupTbl>
    {
        public CattleGroupTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.name).Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.code).Nullable();
            Map(x => x.date).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.UserName).Nullable();
            Map(x => x.UserIdentity).Nullable();

            Table("SmartCattle.CattleGroupTbl");
        }
    }
}