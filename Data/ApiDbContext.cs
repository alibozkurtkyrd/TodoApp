using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Mapping;
using TodoApp.Model;

namespace TodoApp.Data
{
    public class ApiDbContext:DbContext
    {
        public virtual DbSet<User> Users {get; set;}
        public virtual DbSet<Todo> Todos {get; set;}

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            :base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new TodoMap());

        }
    }
}