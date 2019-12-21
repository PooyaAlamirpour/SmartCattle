using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    class ActivityStateTbl
    {
        public virtual int ID { get; protected set; }
        public virtual decimal Sitting { get; set; }
        public virtual decimal Standing { get; set; }
        public virtual decimal Walking { get; set; }
        public virtual decimal Eating { get; set; }
        public virtual decimal Rumination { get; set; }
        public virtual decimal Drinking { get; set; }
        public virtual int cattleId { get; set; }
        public virtual DateTime date { get; set; }
        public virtual int FarmID { get; set; }
        public virtual long LastRecievedId { get; set; }
    }

    class ActivityStateTblMapping : ClassMap<ActivityStateTbl>
    {
        public ActivityStateTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Sitting).Not.Nullable().Length(18);
            Map(x => x.Standing).Not.Nullable().Length(18);
            Map(x => x.Walking).Not.Nullable().Length(18);
            Map(x => x.Eating).Not.Nullable().Length(18);
            Map(x => x.Rumination).Not.Nullable().Length(18);
            Map(x => x.Drinking).Not.Nullable().Length(18);
            Map(x => x.cattleId).Not.Nullable();
            Map(x => x.date).Not.Nullable().Length(7);
            Map(x => x.FarmID).Not.Nullable();
            Map(x => x.LastRecievedId).Not.Nullable();
            Table("SmartCattle.ActivityStateTbl");
        }
    }
}
