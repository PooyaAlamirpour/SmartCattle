using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    class ExceptionTbl
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
            Map(x => x.Message).Not.Nullable();
            Map(x => x.Value).Not.Nullable();
            Map(x => x.Date).Not.Nullable();

            Table("SmartCattle.ExceptionTbl");
        }
    }
}
