using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattleCoreProcessor.Domain
{
    class CattleNotificationsSetting
    {
        public virtual int ID { get; set; }
        public virtual int FarmId { get; set; }
        public virtual string GroupName { get; set; }
        public virtual string Topic { get; set; }
        public virtual string Roles { get; set; }
        public virtual string Comment { get; set; }
        public virtual String SnoozeTime { get; set; }
        public virtual double PeroidTime { get; set; }
        public virtual double WindowTime { get; set; }
        public virtual double CattleTempMin { get; set; }
        public virtual double CattleTempMax { get; set; }
        public virtual double SittingMin { get; set; }
        public virtual double SittingMax { get; set; }
        public virtual double WalkingMin { get; set; }
        public virtual double WalkingMax { get; set; }
        public virtual double RuminationMin { get; set; }
        public virtual double RuminationMax { get; set; }
        public virtual double DrinkingMin { get; set; }
        public virtual double DrinkingMax { get; set; }
        public virtual double EatingMin { get; set; }
        public virtual double EatingMax { get; set; }
        public virtual double StandingMin { get; set; }
        public virtual double StandingMax { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual String ActivationState { get; set; }
    }

    class CattleNotificationsSettingMapping : ClassMap<CattleNotificationsSetting>
    {
        public CattleNotificationsSettingMapping()
        {
            Id(x => x.ID);
            Map(x => x.ID).Nullable();
            Map(x => x.FarmId).Nullable();
            Map(x => x.GroupName).Nullable();
            Map(x => x.Topic).Nullable();
            Map(x => x.Roles).Nullable();
            Map(x => x.Comment).Nullable();
            Map(x => x.SnoozeTime).Nullable();
            Map(x => x.PeroidTime).Nullable();
            Map(x => x.WindowTime).Nullable();
            Map(x => x.CattleTempMin).Nullable();
            Map(x => x.CattleTempMax).Nullable();
            Map(x => x.SittingMin).Nullable();
            Map(x => x.SittingMax).Nullable();
            Map(x => x.WalkingMin).Nullable();
            Map(x => x.WalkingMax).Nullable();
            Map(x => x.RuminationMin).Nullable();
            Map(x => x.RuminationMax).Nullable();
            Map(x => x.DrinkingMin).Nullable();
            Map(x => x.DrinkingMax).Nullable();
            Map(x => x.EatingMin).Nullable();
            Map(x => x.EatingMax).Nullable();
            Map(x => x.StandingMin).Nullable();
            Map(x => x.StandingMax).Nullable();
            Map(x => x.CreateDate).Nullable();
            Map(x => x.ActivationState).Nullable();

            Table("SmartCattle.CattleNotificationsSetting");
        }
    }
}

