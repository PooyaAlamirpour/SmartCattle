using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCattle.DataAccess;
using SmartCattle.Core.DTO;
using SmartCattle.DomainClass;
using SmartCattle.Core;

namespace SmartCattle.Service
{
    public class CattleTHIService
    {
        GenericUnitOfWork<CattleTHI> uow;
        public CattleTHIService(GenericUnitOfWork<CattleTHI> uow)
        { 
            this.uow = uow;
        }
        public void setValue(CattleTHIDTO model)
        {
            CattleTHI cattleThi = Mapper<CattleTHIDTO, CattleTHI>.Map(model);
            uow.GenericRepository.Insert(cattleThi);
        }
        //public double getValue(int CattleId, DateTime Date)
        //{
        //    double THI=0;
        //    var row = uow.GenericRepository.Get(c => c.CattleID == CattleId &&
        //    c.date.ToShortDateString().Equals(Date.ToShortDateString()))
        //   .Result.FirstOrDefault();
        //    if (row != null)
        //    {
        //        THI = row.TdbValue - (0.55 - (0.55 * row.RHValue/100)) * (row.TdbValue - 58);
        //    }
        //    return THI;
        //}
    }
}
