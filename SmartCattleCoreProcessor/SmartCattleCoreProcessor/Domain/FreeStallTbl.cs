using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Domain
{
    public class FreeStallTbl
    {
        public virtual int ID { get; set; }
        public virtual string name { get; set; }
        public virtual string Description { get; set; }
        public virtual int code { get; set; }
        public virtual float location { get; set; }
        public virtual int FarmID { get; set; }
        public virtual int GroupID { get; set; }
        public virtual string UserId { get; set; }
    }

    class FreeStallTblMapping : ClassMap<FreeStallTbl>
    {
        public FreeStallTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.name).Nullable();
            Map(x => x.Description).Not.Nullable();
            Map(x => x.code).Not.Nullable();
            Map(x => x.location).Not.Nullable();
            Map(x => x.FarmID).Not.Nullable();
            Map(x => x.GroupID).Not.Nullable();
            Map(x => x.UserId).Not.Nullable();

            Table("SmartCattle.FreeStallTbl");
        }
    }
}
