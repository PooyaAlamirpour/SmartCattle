using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.Core
{
   public class Mapper<TSource,TDest> where TDest : class where TSource:class
    { 
        public static TDest Map (TSource Source)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDest>());
            return Mapper.Map<TDest>(Source);
        }
    }
}
