using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Domain
{
    class ActivityStateTbl
    {
        public virtual int ID { get; set; }
        public virtual String jsonedActivities { get; set; }
        public virtual double Sitting { get; set; }
        public virtual double Standing { get; set; }
        public virtual double Walking { get; set; }
        public virtual double Eating { get; set; }
        public virtual double Rumination { get; set; }
        public virtual double Drinking { get; set; }
        public virtual int cattleId { get; set; }
        public virtual DateTime date { get; set; }
        public virtual int FarmID { get; set; }
        public virtual long LastRecievedId { get; set; }
        public virtual String UserId { get; set; }
    }

    class ActivityStateTblMapping : ClassMap<ActivityStateTbl>
    {
        public ActivityStateTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.jsonedActivities).Nullable();
            Map(x => x.Sitting).Not.Nullable();
            Map(x => x.Standing).Not.Nullable();
            Map(x => x.Walking).Not.Nullable();
            Map(x => x.Eating).Not.Nullable();
            Map(x => x.Rumination).Not.Nullable();
            Map(x => x.Drinking).Not.Nullable();
            Map(x => x.cattleId).Not.Nullable();
            Map(x => x.date).Not.Nullable();
            Map(x => x.FarmID).Not.Nullable();
            Map(x => x.LastRecievedId).Nullable();
            Map(x => x.UserId).Nullable();

            Table("SmartCattle.ActivityStateTbl");
        }
    }
}