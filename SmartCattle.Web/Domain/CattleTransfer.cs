using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Domain
{
    public class CattleTransfer
    {
        public virtual int ID { get; set; }
        public virtual int CattleID { get; set; }
        public virtual String Topic { get; set; }
        public virtual String Description { get; set; }
        public virtual int NewTopicID { get; set; }
        public virtual int OldTopicID { get; set; }
        public virtual String NewTopicName { get; set; }
        public virtual String OldTopicName { get; set; }
        public virtual int FarmID { get; set; }
        public virtual DateTime date { get; set; }
        public virtual String UserName { get; set; }
        public virtual int UserIdentity { get; set; }
    }

    class CattleTransferMapping : ClassMap<CattleTransfer>
    {
        public CattleTransferMapping()
        {
            Id(x => x.ID);
            Map(x => x.CattleID).Nullable();
            Map(x => x.Topic).Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.NewTopicID).Nullable();
            Map(x => x.OldTopicID).Nullable();
            Map(x => x.NewTopicName).Nullable();
            Map(x => x.OldTopicName).Nullable();
            Map(x => x.FarmID).Nullable();
            Map(x => x.date).Nullable();
            Map(x => x.UserName).Nullable();
            Map(x => x.UserIdentity).Nullable();

            Table("SmartCattle.CattleTransfer");
        }
    }
}