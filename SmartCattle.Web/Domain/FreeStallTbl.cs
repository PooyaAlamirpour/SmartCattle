using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class FreeStallTbl
    {
        public virtual int ID { get; set; }
        public virtual String name { get; set; }
        public virtual String Description { get; set; }
        public virtual int ServerName { get; set; }
        public virtual int FarmID { get; set; }
        public virtual int GroupID { get; set; }
        public virtual String UserId { get; set; }
    }

    class FreeStallTblMapping : ClassMap<FreeStallTbl>
    {
        public FreeStallTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.name).Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.ServerName).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.GroupID).Nullable();
            Map(x => x.UserId).Nullable();

            Table("SmartCattle.FreeStallTbl");
        }
    }
}