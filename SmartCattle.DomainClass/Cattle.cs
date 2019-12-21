using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
    public class Cattle : BaseEntity
    {   
        public Cattle()
        {
            FertilityStates = new List<CattleFrtilityState>();
        }
        public int age { get; set; }
        public int preg { get; set; }
        public double milkAvg { get; set; } 
        public HealthState healthStatus { get; set; }
        public int animalNumber { get; set; }   
        public HeatState heatStatus { get; set; }
        public DateTime birthDate { get; set; }
        public int Dim { get; set; }
        public FertilityStates fertilityStatus { get; set; }
        public int lactationNumber { get; set; } 
        public int  InseminationCount { get; set; }
        public DateTime? lastInseminationDate { get; set; }
        public DateTime? lastCalvingDate { get; set; } 
        public int calvingCount { set; get; }
        public int? CattleGroupId { set; get; }
        public int FreeStallId { get; set;}
        public int FarmID { get; set; } 
        public virtual CattleGroup CattleGroup { set; get; }
        public virtual CattleHerd CattleHerd { set; get; }
        public virtual SmartCattleUser User { get; set; }
        public virtual Farm Farm { get; set; }
        public virtual ICollection<CattleMilking> Milkings {set; get;}  
        public virtual ICollection<CattleActivityState> ActivityState { set; get; }
        public virtual ICollection<CattleHealthState> HealthStates { set; get; } 
        public virtual ICollection<CattleEvent> Events { set; get; }
        public virtual ICollection<CattleTempreture> Tempretures { set; get; } 
        public virtual ICollection<CattleFrtilityState> FertilityStates { set; get; }
        public virtual ICollection<CattlePosition> Positions { set; get; } 
        public virtual ICollection<CattleHeatState> HeatStates { set; get; }
        public virtual ICollection<CattleTHI> CattleTHIs { set; get; }
        public virtual ICollection<TimeBudget> CattleTimeBudgets { set; get; }
        public virtual ICollection<CattleScore> CattleScores { set; get; }
    }
}
