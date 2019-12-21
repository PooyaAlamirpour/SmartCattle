using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class RolesList_UserTbl
    {
        public virtual int ID { get; set; }
        public virtual string jName { get; set; }
        public virtual string uId { get; set; }
        public virtual string Permissions { get; set; }
        public virtual string Comment { get; set; }
        public virtual int FarmId { get; set; }
    }

    class RolesList_UserTblMapping : ClassMap<RolesList_UserTbl>
    {
        public RolesList_UserTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.jName).Nullable();
            Map(x => x.uId).Nullable();
            Map(x => x.Permissions).Nullable();
            Map(x => x.Comment).Nullable();
            Map(x => x.FarmId).Nullable();

            Table("SmartCattle.RolesList_UserTbl");
        }
    }
}