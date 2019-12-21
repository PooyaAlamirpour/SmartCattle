using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattlePositionTbl
    {
        public virtual int ID { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual int cattleId { get; set; }
        public virtual DateTime date { get; set; }
        public virtual int LastRecievedId { get; set; }
        public virtual int FarmID { get; set; }
        public virtual int FreeStall { get; set; }
    }

    class CattlePositionTblMapping : ClassMap<CattlePositionTbl>
    {
        public CattlePositionTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Latitude).Nullable();
            Map(x => x.Longitude).Nullable();
            Map(x => x.cattleId).Nullable();
            Map(x => x.date).Nullable();
            Map(x => x.LastRecievedId).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.FreeStall).Nullable();

            Table("SmartCattle.CattlePositionTbl");
        }
    }
}