using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Domain
{
    class NotificationsTable
    {
        public virtual int ID { get; set; }
        public virtual String uId { get; set; }
        public virtual String Topic { get; set; }
        public virtual String Comment { get; set; }
        public virtual int FarmID { get; set; }
        public virtual String RoleName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual String Status { get; set; }
        public virtual String NotificationType { get; set; }
        public virtual double Snooze { get; set; }
        public virtual String TagName { get; set; }
        public virtual String SnoozeMsg { get; set; }
        public virtual String Username { get; set; }

        public virtual int Cattle_Freestall_Id { get; set; }
        public virtual String NotificationGroup { get; set; }
        public virtual String Act { get; set; }
        public virtual DateTime? DeactiveAt { get; set; }
        public virtual DateTime? ActDate { get; set; }
        public virtual String ActionComment { get; set; }
        public virtual String Deactive { get; set; }
    }

    class NotificationsMapping : ClassMap<NotificationsTable>
    {
        public NotificationsMapping()
        {
            Id(x => x.ID);
            Map(x => x.uId).Nullable();
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
            Map(x => x.Deactive).Unique();

            Table("SmartCattle.NotificationsTable");
        }
    }
}