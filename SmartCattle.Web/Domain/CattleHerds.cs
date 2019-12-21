using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattleHerds
    {
        public virtual int ID { get; set; }
        public virtual String name { get; set; }
        public virtual String Description { get; set; }
        public virtual int FarmID { get; set; }
        public virtual String UserName { get; set; }
        public virtual int UserIdentity { get; set; }
        public virtual DateTime date { get; set; }
    }

    class CattleHerdsMapping : ClassMap<CattleHerds>
    {
        public CattleHerdsMapping()
        {
            Id(x => x.ID);
            Map(x => x.name).Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.UserName).Nullable();
            Map(x => x.UserIdentity).Nullable();
            Map(x => x.date).Nullable();

            Table("SmartCattle.CattleHerds");
        }
    }
}