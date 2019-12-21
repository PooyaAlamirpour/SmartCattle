using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class FreeStallNotificationsSetting
    {
        public virtual int ID { get; set; }
        public virtual int FarmId { get; set; }
        public virtual string GroupName { get; set; }
        public virtual string Topic { get; set; }
        public virtual string Roles { get; set; }
        public virtual string Comment { get; set; }
        public virtual int PeroidTime { get; set; }
        public virtual int WindowTime { get; set; }
        public virtual double TempMin { get; set; }
        public virtual double TempMax { get; set; }
        public virtual double HumMin { get; set; }
        public virtual double HumMax { get; set; }
        public virtual double THIMin { get; set; }
        public virtual double THIMax { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual string ActivationState { get; set; }
        public virtual string SnoozeTime { get; set; }
    }

    class FreeStallNotificationsSettingMapping : ClassMap<FreeStallNotificationsSetting>
    {
        public FreeStallNotificationsSettingMapping()
        {
            Id(x => x.ID);
            Map(x => x.FarmId).Nullable();
            Map(x => x.GroupName).Nullable();
            Map(x => x.Topic).Nullable();
            Map(x => x.Roles).Nullable();
            Map(x => x.Comment).Nullable();
            Map(x => x.PeroidTime).Nullable();
            Map(x => x.WindowTime).Nullable();
            Map(x => x.TempMin).Nullable();
            Map(x => x.TempMax).Nullable();
            Map(x => x.HumMin).Nullable();
            Map(x => x.HumMax).Nullable();
            Map(x => x.THIMin).Nullable();
            Map(x => x.THIMax).Nullable();
            Map(x => x.CreateDate).Nullable();
            Map(x => x.ActivationState).Nullable();
            Map(x => x.SnoozeTime).Nullable();

            Table("SmartCattle.FreeStallNotificationsSetting");
        }
    }
}