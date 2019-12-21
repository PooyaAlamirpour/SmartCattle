using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Themes.Bootstrap.Domain
{
    class NotificationsTable
    {
        public virtual int ID { get; set; }
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
    }

    class NotificationsMapping : ClassMap<NotificationsTable>
    {
        public NotificationsMapping()
        {
            Id(x => x.ID);
            Map(x => x.Topic).Not.Nullable();
            Map(x => x.Comment).Not.Nullable();
            Map(x => x.FarmID).Not.Nullable();
            Map(x => x.RoleName).Not.Nullable();
            Map(x => x.CreatedDate).Not.Nullable();
            Map(x => x.Status).Not.Nullable();
            Map(x => x.NotificationType).Not.Nullable();
            Map(x => x.Snooze).Not.Nullable();
            Map(x => x.TagName).Not.Nullable();
            Map(x => x.SnoozeMsg).Not.Nullable();
            Map(x => x.Username).Not.Nullable();
            Table("SmartCattle.NotificationsTable");
        }
    }
}
