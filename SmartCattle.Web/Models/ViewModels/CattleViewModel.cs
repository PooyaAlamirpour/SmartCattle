using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class CattleViewModel
    { 
        [Required]
        public int animalNumber { get; set; }
        public String Name { get; set; }
        [Required]
        public string Sex { set; get; }
        public String Genetics_type_num { get; set; }
        public String birthDate { get; set; }
        public int MotherID { get; set; }
        public String lastCalvingDate { get; set; }
        public int lactationNumber { get; set; }
    }
}