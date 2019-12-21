using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartCattle.Core;
using SmartCattle.DataAccess;
using System.Linq.Expressions;

namespace SmartCattle.Service
{
   public class BaseServices<TEntity> : IService<TEntity> , IDisposable where TEntity : BaseEntity
    {
        GenericUnitOfWork<TEntity> uow;
        public BaseServices(GenericUnitOfWork<TEntity> uow)
        {
            this.uow = uow;
        }
        public async Task<ActionMessage> Delete(TEntity entity)
        {
             uow.GenericRepository.Delete(entity);
            if (await uow.Save() > 0)
            {
                return new ActionMessage() { title ="Deleted_successfully", value = entity.ID, type = messageType.success };
            }
            else
            {
                return new ActionMessage() { title = "Delete_faild", value = -1, type = messageType.error };
            }
        }
        public async Task<ActionMessage> DeleteById(object ID)
        {
            uow.GenericRepository.DeleteById(ID);
            if (await uow.Save() > 0)
            {
                return new ActionMessage() { title = "Deleted_successfully", value = ID, type = messageType.success };
            }
            else
            {
                return new ActionMessage() { title = "Delete_faild", value = -1, type = messageType.error };
            }           
        }

        public async Task<ActionMessage> Insert(TEntity entity)
        {
            uow.GenericRepository.Insert(entity);
            if (await uow.Save() > 0)
            {
                return new ActionMessage() { title = "Added_successfully", value = entity.ID, type = messageType.success };
            }
            else
            {
                return new ActionMessage() { title = "Add_faild", value = -1, type = messageType.error };
            }
        }

        public async Task<ActionMessage> InsertMany(IEnumerable<TEntity> entities)
        {
            uow.GenericRepository.InsertMany(entities);
            if (await uow.Save() > 0)
            {
                return new ActionMessage() { title = "Added_successfully", value = entities.Count(), type = messageType.success };
            }
            else
            {
                return new ActionMessage() { title = "Add_faild", value = -1, type = messageType.error };
            }
        }

        public async Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> filter = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "" ,int skip=0 , int take=-1)
        {

            return await uow.GenericRepository.Get(filter, orderBy, includeProperties, skip, take);
        }

        public IEnumerable<IGrouping<object , TEntity>> GroupBy(Func<TEntity, object> gruopby
            , Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {

            return  uow.GenericRepository.GroupBy(gruopby,filter , includeProperties);
        }

        public  TEntity GetById(int? ID, string includeProperties = "")
        {
            return  uow.GenericRepository.GetById(ID, includeProperties);
        }
        public async Task<int> TotalNumber(Expression<Func<TEntity, bool>> filter = null)
        {
            return await uow.GenericRepository.TotalNumber(filter);
        }

        public async Task<ActionMessage> Update(TEntity entity)
        {
            uow.GenericRepository.Update(entity);
            if (await uow.Save() > 0)
            {
                return new ActionMessage() { title = "Updateed_successfully", value = entity.ID, type = messageType.success };
            }
            else
            {
                return new ActionMessage() { title = "Update_faild", value = -1, type = messageType.error };
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    uow.Dispose();
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
