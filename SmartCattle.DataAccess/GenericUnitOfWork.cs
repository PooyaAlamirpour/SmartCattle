using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DataAccess
{
  public class GenericUnitOfWork<TEntity> : IDisposable, IUnitOfWork<TEntity> where TEntity: BaseEntity 
    {
        private readonly IDbContext context;
        public GenericUnitOfWork(IDbContext context)
        {
            this.context = context;
           
        }
        private GenericRepository<TEntity> genericRepository;

        public GenericRepository<TEntity> GenericRepository
        {
            get
            {
                if (this.genericRepository == null)
                {
                    return new GenericRepository<TEntity>(context);
                }
                return genericRepository;
            }
        }

        public async Task<int> Save()
        {
            try
            {
                return await ((SmartCattleContext)context).SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
