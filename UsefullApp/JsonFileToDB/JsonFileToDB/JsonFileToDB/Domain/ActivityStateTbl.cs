using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFileToDB.Domain
{
    public class ActivityStateTbl
    {
        public virtual int ID { get; set; }
        public virtual string jsonedActivities { get; set; }
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
        public virtual string UserId { get; set; }
    }

    class ActivityStateTblMapping : ClassMap<ActivityStateTbl>
    {
        public ActivityStateTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.jsonedActivities).Nullable();
            Map(x => x.Sitting).Nullable();
            Map(x => x.Standing).Nullable();
            Map(x => x.Walking).Nullable();
            Map(x => x.Eating).Nullable();
            Map(x => x.Rumination).Nullable();
            Map(x => x.Drinking).Nullable();
            Map(x => x.cattleId).Nullable();
            Map(x => x.date).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.LastRecievedId).Nullable();
            Map(x => x.UserId).Nullable();

            Table("SmartCattle.ActivityStateTbl");
        }
    }
}
