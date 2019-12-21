using SmartCattle.Core;
using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.Service
{
   public interface IService<TEntity> where TEntity:BaseEntity
    {
        Task<ActionMessage> Insert(TEntity entity);
        Task<ActionMessage> Update(TEntity entity);
        Task<ActionMessage> Delete(TEntity entity);
        Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> filter = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "",int skip=0 , int take = -1);
    }
}
