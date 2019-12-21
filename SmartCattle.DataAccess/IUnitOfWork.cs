using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DataAccess
{
  public interface IUnitOfWork<TEntity> where TEntity:BaseEntity
    {
    }
}
