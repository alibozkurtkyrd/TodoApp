using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace TodoApp.Data.Abstract
{
    public interface IRepository<T>
    {
         T GetById(int id);

        List<T> GetAll();

        void Create(T entity);

        void Update(T entity);
        void Delete(T entity);
    }
}