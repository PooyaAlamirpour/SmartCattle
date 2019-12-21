using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class ActionControllerListTbl
    {
        public virtual int ID { get; set; }
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual string Comment { get; set; }
        public virtual string UniqueId { get; set; }
        public virtual bool PartialView { get; set; }
    }

    class ActionControllerListTblMapping : ClassMap<ActionControllerListTbl>
    {
        public ActionControllerListTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Controller).Nullable();
            Map(x => x.Action).Nullable();
            Map(x => x.Comment).Nullable();
            Map(x => x.UniqueId).Nullable();
            Map(x => x.PartialView).Nullable();

            Table("SmartCattle.ActionControllerListTbl");
        }
    }
}