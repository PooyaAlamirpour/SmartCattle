﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
   public class FarmPlaces:BaseEntity
    {
        public string Name { get; set; }
        PlaceType Type { get; set; }
        public string Description { get; set; }

    }

    public enum PlaceType
    {
        Stall,
        FreeStall,
        Hospital,
        MilkingParlor,
        Alley
    }
}
