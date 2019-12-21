using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Domain
{
    public class CurrentValue
    {
        public virtual int ID { get; set; }
        public virtual String ValueName { get; set; }
        public virtual double Value { get; set; }
        public virtual DateTime LastComputationDate { get; set; }
        public virtual int FarmId { get; set; }
    }

    class CurrentValueMapping : ClassMap<CurrentValue>
    {
        public CurrentValueMapping()
        {
            Id(x => x.ID);
            Map(x => x.ValueName).Nullable();
            Map(x => x.Value).Nullable();
            Map(x => x.LastComputationDate).Nullable();
            Map(x => x.FarmId).Nullable();

            Table("SmartCattle.CurrentValue");
        }
    }
}
