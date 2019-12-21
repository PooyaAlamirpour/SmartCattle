using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DataAccess
{
   public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : BaseEntity;
        int saveChanges();
    }
}
