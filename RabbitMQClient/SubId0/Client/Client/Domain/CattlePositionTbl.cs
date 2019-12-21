using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    class CattlePositionTbl
    {
        public virtual int ID { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual int cattleId { get; set; }
        public virtual DateTime date { get; set; }
        public virtual long LastRecievedId { get; set; }
        public virtual int FarmID { get; set; }
        public virtual int FreeStall { get; set; }
    }

    class CattlePositionTblMapping : ClassMap<CattlePositionTbl>
    {
        public CattlePositionTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Latitude).Not.Nullable();
            Map(x => x.Longitude).Not.Nullable();
            Map(x => x.cattleId).Not.Nullable();
            Map(x => x.date).Not.Nullable();
            Map(x => x.LastRecievedId).Not.Nullable();
            Map(x => x.FarmID).Not.Nullable();
            Map(x => x.FreeStall).Not.Nullable();
            Table("SmartCattle.CattlePositionTbl");
        }
    }
}
