using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class ExceptionTbl
    {
        public virtual int ID { get; set; }
        public virtual String Message { get; set; }
        public virtual String Value { get; set; }
        public virtual DateTime Date { get; set; }
    }

    class ExceptionTblMapping : ClassMap<ExceptionTbl>
    {
        public ExceptionTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.Message).Nullable();
            Map(x => x.Value).Nullable();
            Map(x => x.Date).Nullable();

            Table("SmartCattle.ExceptionTbl");
        }
    }
}