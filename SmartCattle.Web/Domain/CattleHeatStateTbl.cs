using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattleHeatStateTbl
    {
        public virtual int ID { get; set; }
        public virtual String Status { get; set; }
        public virtual String Value { get; set; }
        public virtual String description { get; set; }
        public virtual int cattleId { get; set; }
        public virtual DateTime date { get; set; }
        public virtual int FarmID { get; set; }
        public virtual String UserName { get; set; }
        public virtual int UserIdentity { get; set; }
    }

    class CattleHeatStateTblMapping : ClassMap<CattleHeatStateTbl>
    {
        public CattleHeatStateTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Status).Nullable();
            Map(x => x.Value).Nullable();
            Map(x => x.description).Nullable();
            Map(x => x.cattleId).Nullable();
            Map(x => x.date).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.UserName).Nullable();
            Map(x => x.UserIdentity).Nullable();

            Table("SmartCattle.CattleHeatStateTbl");
        }
    }
}