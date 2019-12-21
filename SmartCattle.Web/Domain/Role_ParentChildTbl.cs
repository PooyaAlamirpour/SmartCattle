using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class Role_ParentChildTbl
    {
        public virtual int ID { get; set; }
        public virtual int ParentId { get; set; }
        public virtual int ParentType { get; set; }
        public virtual int ChildId { get; set; }
        public virtual int ChildType { get; set; }
        public virtual int FarmId { get; set; }
        public virtual int SubId { get; set; }
    }

    class Role_ParentChildTblMapping : ClassMap<Role_ParentChildTbl>
    {
        public Role_ParentChildTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.ParentId).Nullable();
            Map(x => x.ParentType).Nullable();
            Map(x => x.ChildId).Nullable();
            Map(x => x.ChildType).Nullable();
            Map(x => x.FarmId).Nullable();
            Map(x => x.SubId).Nullable();

            Table("SmartCattle.Role_ParentChildTbl");
        }
    }
}