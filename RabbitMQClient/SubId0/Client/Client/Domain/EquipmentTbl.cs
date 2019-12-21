using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain
{
    public class EquipmentTbl
    {
        public virtual int ID { get; set; }
        public virtual string DeviceCategory { get; set; }
        public virtual string projectName { get; set; }
        public virtual int subId { get; set; }
        public virtual string DeviceType { get; set; }
        public virtual string DeviceSubtype { get; set; }
        public virtual string PacketType { get; set; }
        public virtual string Version { get; set; }
        public virtual string PowerType { get; set; }
        public virtual int Equipmentid { get; set; }
        public virtual string Mac { get; set; }
        public virtual string Projectid { get; set; }
        public virtual string Zoneid { get; set; }
        public virtual string Locationx { get; set; }
        public virtual string Locationy { get; set; }
        public virtual string Locationz { get; set; }
        public virtual string Date1 { get; set; }
        public virtual string Date2 { get; set; }
        public virtual string Reserved1 { get; set; }
    }

    class EquipmentTblMapping : ClassMap<EquipmentTbl>
    {
        public EquipmentTblMapping()
        {
            Id(x => x.ID);
            Map(x => x.DeviceCategory).Nullable();
            Map(x => x.projectName).Nullable();
            Map(x => x.subId).Nullable();
            Map(x => x.DeviceType).Nullable();
            Map(x => x.DeviceSubtype).Nullable();
            Map(x => x.PacketType).Nullable();
            Map(x => x.Version).Nullable();
            Map(x => x.PowerType).Nullable();
            Map(x => x.Equipmentid).Nullable();
            Map(x => x.Mac).Nullable();
            Map(x => x.Projectid).Nullable();
            Map(x => x.Zoneid).Nullable();
            Map(x => x.Locationx).Nullable();
            Map(x => x.Locationy).Nullable();
            Map(x => x.Locationz).Nullable();
            Map(x => x.Date1).Nullable();
            Map(x => x.Date2).Nullable();
            Map(x => x.Reserved1).Nullable();

            Table("SmartCattle.EquipmentTbl");
        }
    }
}
