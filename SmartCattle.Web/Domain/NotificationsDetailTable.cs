using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class NotificationsDetailTable
    {
        public virtual int ID { get; set; }
        public virtual int NotificationID { get; set; }
        public virtual String Topic { get; set; }
        public virtual String Comment { get; set; }
        public virtual int FarmID { get; set; }
        public virtual String RoleName { get; set; }
        public virtual String CreatedDate { get; set; }
        public virtual String Status { get; set; }
        public virtual String NotificationType { get; set; }
        public virtual int Snooze { get; set; }
        public virtual String TagName { get; set; }
        public virtual String SnoozeMsg { get; set; }
        public virtual String Username { get; set; }
        public virtual int Cattle_Freestall_Id { get; set; }
        public virtual String NotificationGroup { get; set; }
        public virtual String Act { get; set; }
        public virtual String DeactiveAt { get; set; }
        public virtual String ActDate { get; set; }
        public virtual String ActionComment { get; set; }
    }

    class NotificationsDetailTableMapping : ClassMap<NotificationsDetailTable>
    {
        public NotificationsDetailTableMapping()
        {
            Id(x => x.ID);
            Map(x => x.NotificationID).Nullable();
            Map(x => x.Topic).Nullable();
            Map(x => x.Comment).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.RoleName).Nullable();
            Map(x => x.CreatedDate).Nullable();
            Map(x => x.Status).Nullable();
            Map(x => x.NotificationType).Nullable();
            Map(x => x.Snooze).Nullable();
            Map(x => x.TagName).Nullable();
            Map(x => x.SnoozeMsg).Nullable();
            Map(x => x.Username).Nullable();
            Map(x => x.Cattle_Freestall_Id).Nullable();
            Map(x => x.NotificationGroup).Nullable();
            Map(x => x.Act).Nullable();
            Map(x => x.DeactiveAt).Nullable();
            Map(x => x.ActDate).Nullable();
            Map(x => x.ActionComment).Nullable();

            Table("SmartCattle.NotificationsDetailTable");
        }
    }
}