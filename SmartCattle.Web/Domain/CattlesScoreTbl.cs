using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattlesScoreTbl
    {
        public virtual int ID { get; set; }
        public virtual double BodyCondition { get; set; }
        public virtual double Cleanliness { get; set; }
        public virtual double Hock { get; set; }
        public virtual double Mobility { get; set; }
        public virtual double Manure { get; set; }
        public virtual double Rumen { get; set; }
        public virtual double Teat { get; set; }
        public virtual int CattleId { get; set; }
        public virtual DateTime Date { get; set; }

        public virtual DateTime BodyConditionDate { get; set; }
        public virtual DateTime CleanlinessDate { get; set; }
        public virtual DateTime HockDate { get; set; }
        public virtual DateTime MobilityDate { get; set; }
        public virtual DateTime ManureDate { get; set; }
        public virtual DateTime RumenDate { get; set; }
        public virtual DateTime TeatDate { get; set; }
    }

    class CattlesScoreTblMapping : ClassMap<CattlesScoreTbl>
    {
        public CattlesScoreTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.BodyCondition).Nullable();
            Map(x => x.Cleanliness).Nullable();
            Map(x => x.Hock).Nullable();
            Map(x => x.Mobility).Nullable();
            Map(x => x.Manure).Nullable();
            Map(x => x.Rumen).Nullable();
            Map(x => x.Teat).Nullable();
            Map(x => x.CattleId).Nullable();
            Map(x => x.Date).Nullable();

            Map(x => x.BodyConditionDate).Nullable();
            Map(x => x.CleanlinessDate).Nullable();
            Map(x => x.HockDate).Nullable();
            Map(x => x.MobilityDate).Nullable();
            Map(x => x.ManureDate).Nullable();
            Map(x => x.RumenDate).Nullable();
            Map(x => x.TeatDate).Nullable();

            Table("SmartCattle.CattlesScoreTbl");
        }
    }
}