using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Domain
{
    class CattleTbl
    {
        public virtual int ID { get; set; }
        public virtual int age { get; set; }
        public virtual int preg { get; set; }
        public virtual double milkAvg { get; set; }
        public virtual int healthStatus { get; set; }
        public virtual int animalNumber { get; set; }
        public virtual int heatStatus { get; set; }
        public virtual DateTime birthDate { get; set; }
        public virtual int Dim { get; set; }
        public virtual int fertilityStatus { get; set; }
        public virtual int lactationNumber { get; set; }
        public virtual int InseminationCount { get; set; }
        public virtual DateTime lastInseminationDate { get; set; }
        public virtual DateTime lastCalvingDate { get; set; }
        public virtual int calvingCount { get; set; }
        public virtual int CattleGroupId { get; set; }
        public virtual int FreeStallId { get; set; }
        public virtual int FarmID { get; set; }
        public virtual DateTime UserId { get; set; }
        public virtual int CattleHerd_ID { get; set; }
    }

    class CattleTblMapping : ClassMap<CattleTbl>
    {
        public CattleTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.age).Nullable();
            Map(x => x.preg).Nullable();
            Map(x => x.milkAvg).Nullable();
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

            Table("SmartCattle.CattleTbl");
        }
    }
}
