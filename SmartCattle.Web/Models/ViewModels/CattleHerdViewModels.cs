using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartCattle.Web.Models.ViewModels
{
    public class CattleHerdViewModels
    {
        public int? HerdId { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
    }
}