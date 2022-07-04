using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Abstract;

namespace TodoApp.Data.Concrete
{
    public class GenericRepository<T> : IRepository<T>
    where T : class
    {
        protected readonly ApiDbContext _context;
        public GenericRepository(ApiDbContext context)
        {
            _context = context;
        }
        public void Create(T entity)
        {
             
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            
        }

        public void Delete(T entity)
        {
            
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            
        }

        public List<T> GetAll()
        {

            return _context.Set<T>().ToList();
            
        }

        public T GetById(int id)
        {

            return _context.Set<T>().Find(id);
            
        }

        public void Update(T entity)
        {

            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            
        }
    }
}