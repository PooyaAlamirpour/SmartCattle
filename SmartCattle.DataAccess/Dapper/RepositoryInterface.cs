using SmartCattle.DomainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DataAccess.Dapper
{
    interface RepositoryInterface<T> where T :BaseEntity
    { 
            IEnumerable<T> GetAll(int skip , int take);
            T Find(int id);
            T Add(T entity);
            T Update(T entity);
            void Remove(int id);         
    }
}
