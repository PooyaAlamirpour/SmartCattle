using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DataAccess
{
   public class GenericRepository<TEntity> : IDisposable where TEntity : BaseEntity 
    {
        IDbContext context;
        DbSet<TEntity> dbSet;

        public GenericRepository(IDbContext _context)
        {
            context = _context;
            dbSet = context.Set<TEntity>();
        }
        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity,bool>> filter=null
            , Func<IQueryable<TEntity>,IOrderedQueryable<TEntity>> orderBy=null,string includeProperties = "",
            int skip =0 , int take =-1 )
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            } 
                   
            if (orderBy != null && take != -1)
            {
                return await orderBy(query).Skip(skip).Take(take).AsNoTracking().ToListAsync();
            }
            if (orderBy == null && take != -1)
            {
                query = query.OrderBy(e=>e.ID).Skip(skip).Take(take);
            }
            if (orderBy != null && take == -1)
            {
                return await orderBy(query).AsNoTracking().ToListAsync();
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public virtual IEnumerable<IGrouping<object,TEntity>> GroupBy(Func<TEntity,object> gruopby 
            ,Expression < Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet; 
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.AsNoTracking().GroupBy(gruopby).AsEnumerable();                          
        }


        public virtual  TEntity GetById(int? Id, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            query = query.Where(q => q.ID == Id);
            foreach (var includeProperty in includeProperties.Split
               (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void InsertMany (IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }
        public virtual void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }
        public virtual void DeleteById(object id)
        {
            TEntity entity = dbSet.Find(id);
            Delete(entity);
        }
        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task<int> TotalNumber(Expression<Func<TEntity, bool>> filter = null)
        {
            if(filter!=null)
            {
                return await dbSet.CountAsync(filter);
            }
           return await dbSet.CountAsync();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    dbSet = null;
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
