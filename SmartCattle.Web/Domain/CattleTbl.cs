using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattleTbl
    {
        public virtual int ID { get; set; }
        public virtual int age { get; set; }
        public virtual int preg { get; set; }
        public virtual double milkAvg { get; set; }
        public virtual DateTime milkAvgDate { get; set; }
        public virtual String healthStatus { get; set; }
        public virtual int animalNumber { get; set; }
        public virtual String heatStatus { get; set; }
        public virtual DateTime birthDate { get; set; }
        public virtual int Dim { get; set; }
        public virtual String fertilityStatus { get; set; }
        public virtual int lactationNumber { get; set; }
        public virtual int InseminationCount { get; set; }
        public virtual DateTime lastInseminationDate { get; set; }
        public virtual DateTime lastCalvingDate { get; set; }
        public virtual int calvingCount { get; set; }
        public virtual int CattleGroupId { get; set; }
        public virtual int FreeStallId { get; set; }
        public virtual int FarmID { get; set; }
        public virtual String UserId { get; set; }
        public virtual int CattleHerd_ID { get; set; }
        public virtual String Sex { get; set; }
        public virtual int MotherID { get; set; }
        public virtual String Genetics_type_num { get; set; }

        public virtual double Body_Condition_Score { get; set; }
        public virtual double Cleanliness { get; set; }
        public virtual double Hock { get; set; }
        public virtual double Mobility { get; set; }
        public virtual double Manure { get; set; }
        public virtual double Rumen { get; set; }
        public virtual double Teat { get; set; }

        public virtual DateTime Body_Condition_ScoreDate { get; set; }
        public virtual DateTime CleanlinessDate { get; set; }
        public virtual DateTime HockDate { get; set; }
        public virtual DateTime MobilityDate { get; set; }
        public virtual DateTime ManureDate { get; set; }
        public virtual DateTime RumenDate { get; set; }
        public virtual DateTime TeatDate { get; set; }
        public virtual DateTime fertilityStatusDate { get; set; }
        public virtual DateTime heatStatusDate { get; set; }
        public virtual DateTime healthStatusDate { get; set; }

        public virtual String Name { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }

    class CattleTblMapping : ClassMap<CattleTbl>
    {
        public CattleTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.age).Nullable();
            Map(x => x.preg).Nullable();
            Map(x => x.milkAvg).Nullable();
            Map(x => x.milkAvgDate).Nullable();
            Map(x => x.healthStatus).Nullable();
            Map(x => x.animalNumber).Nullable();
            Map(x => x.heatStatus).Nullable();
            Map(x => x.birthDate).Nullable();
            Map(x => x.Dim).Nullable();
            Map(x => x.fertilityStatus).Nullable();
            Map(x => x.lactationNumber).Nullable();
            Map(x => x.InseminationCount).Nullable();
            Map(x => x.lastInseminationDate).Nullable();
            Map(x => x.lastCalvingDate).Nullable();
            Map(x => x.calvingCount).Nullable();
            Map(x => x.CattleGroupId).Nullable();
            Map(x => x.FreeStallId).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.UserId).Nullable();
            Map(x => x.CattleHerd_ID).Nullable();
            Map(x => x.Sex).Nullable();
            Map(x => x.MotherID).Nullable();
            Map(x => x.Genetics_type_num).Nullable();

            Map(x => x.Body_Condition_Score).Nullable();
            Map(x => x.Cleanliness).Nullable();
            Map(x => x.Hock).Nullable();
            Map(x => x.Mobility).Nullable();
            Map(x => x.Manure).Nullable();
            Map(x => x.Rumen).Nullable();
            Map(x => x.Teat).Nullable();

            Map(x => x.Body_Condition_ScoreDate).Nullable();
            Map(x => x.CleanlinessDate).Nullable();
            Map(x => x.HockDate).Nullable();
            Map(x => x.MobilityDate).Nullable();
            Map(x => x.ManureDate).Nullable();
            Map(x => x.RumenDate).Nullable();
            Map(x => x.TeatDate).Nullable();
            Map(x => x.fertilityStatusDate).Nullable();
            Map(x => x.heatStatusDate).Nullable();
            Map(x => x.healthStatusDate).Nullable();

            Map(x => x.Name).Nullable();
            Map(x => x.CreatedDate).Nullable();

            Table("SmartCattle.CattleTbl");
        }
    }
}