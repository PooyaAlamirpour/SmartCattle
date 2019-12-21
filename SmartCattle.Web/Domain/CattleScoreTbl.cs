using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattleScoreTbl
    {
        public virtual int ID { get; set; }
        public virtual String item { get; set; }
        public virtual double value { get; set; }
        public virtual int CattleId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int UserId { get; set; }
        public virtual String UserName { get; set; }
    }

    class CattleScoreTblMapping : ClassMap<CattleScoreTbl>
    {
        public CattleScoreTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.item).Nullable();
            Map(x => x.value).Nullable();
            Map(x => x.CattleId).Nullable();
            Map(x => x.Date).Nullable();
            Map(x => x.UserId).Nullable();
            Map(x => x.UserName).Nullable();

            Table("SmartCattle.CattleScoreTbl");
        }
    }
}