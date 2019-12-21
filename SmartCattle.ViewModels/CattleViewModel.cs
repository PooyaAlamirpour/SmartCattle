using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.ViewModels
{
  public class CattleViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string sex { set; get; }
        public int age { get; set; }
        public int lactationValue { get; set; }
        public string sireName { get; set; }
        public string healthStatus { get; set; }
        public int animalNumber { get; set; }
        public bool isYoungStock { get; set; }
        public bool isDry { set; get; }
        [Required]
        public int registerNumber { get; set; }
        public string heatStatus { get; set; }
        public DateTime birthDate { get; set; }
        public int Dim { get; set; }
        public string fertilityStatus { get; set; }
        public int lactationNumber { get; set; }
        public int daysBeforeCalving { get; set; }
        public int daysSinceLastBreeding { get; set; }
        public int numberOfInsemination { get; set; }
        public DateTime? lastCalvingDate { get; set; }
        public DateTime? lastDryOffDate { get; set; }
        public DateTime? lastAlertDate { get; set; }
        public int? CattleGroupId { set; get; }
        public virtual CattleGroupViewModel CattleGroup { set; get; }
    }

    internal class RequiredAttribute : Attribute
    {
    }
}
