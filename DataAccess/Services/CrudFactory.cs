using System.Collections.Generic;
using DataAccess.DAOs;

namespace DataAccess.CRUD
{
    public abstract class CrudFactory<T>
    {
        protected SqlDao _sqlDao;

        public abstract void Create(T entity);
        public abstract void Update(T entity);
        public abstract void Delete(int id);
        public abstract T Retrieve(int id);
        public abstract List<T> RetrieveAll();
    }
}