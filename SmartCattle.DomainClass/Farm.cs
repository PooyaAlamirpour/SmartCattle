using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class Farm:BaseEntity
    { 
        public string name { get; set; }
        public string LogoUrl { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; } 
        public string MapGeoJson { get; set; }
        public string MapFilePath { get; set; }
        public virtual ICollection<FarmSetting> FarmSetting { set; get; }
        public virtual ICollection<Cattle> Cattles { get; set; }
        public virtual ICollection<CattleGroup> Groups { get; set; }
        public virtual ICollection<CattleHerd> Herds { get; set; }
        public virtual ICollection<HerdEvent> HerdEvents { get; set; }
        public virtual ICollection<SmartCattleUser> Users { get; set; } 
        public virtual ICollection<CattleMilking> Milkings { set; get; } 
        public virtual ICollection<CattleActivityState> ActivityState { set; get; }
        public virtual ICollection<CattleHealthState> HealthStates { set; get; }
        public virtual ICollection<CattleEvent> Events { set; get; }
        public virtual ICollection<CattleTempreture> Tempretures { set; get; } 
        public virtual ICollection<CattleFrtilityState> FertilityStates { set; get; }
        public virtual ICollection<CattlePosition> Positions { set; get; }
        public virtual ICollection<CattleHeatState> HeatStates { set; get; }
        public virtual ICollection<CattleTHI> CattleTHIs { set; get; }
        public virtual ICollection<GroupTimeBudget> GroupsTimeBudgets { set; get; }
        public virtual ICollection<Sensor> Sensors { set; get; }
        public virtual ICollection<Notification> Notifications { set; get; }
    }
}
