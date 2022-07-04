using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Abstract;
using TodoApp.Dto;
using TodoApp.Model;

namespace TodoApp.Data.Concrete
{
    public class TodoRepository :
        GenericRepository<Todo>, ITodoRepository
    {
        public TodoRepository(ApiDbContext context): base(context)
        {
 
        }

        public List<GetAllTodoDto> GetAllTodosWithUser()
        {
            //left inner join
            var todos = _context.Todos 
                .Include(t => t.User) // navigation property included
                .Select(t1 => new  GetAllTodoDto
                {
                    Id = t1.Id,
                    TaskName = t1.TaskName,
                    UserName = t1.User.Name,  // user cannot be null becase it is checked in CreateTodo function
                    IsComplete = t1.IsComplete,
                    DeadLine = t1.DeadLine,
                    UserId = t1.UserId               
                })
                .OrderBy(t1 => t1.IsComplete) 
                .ThenBy(t1 => t1.DeadLine)   // The todo list sorting is done according to the "task completion status" and secondly according to the "deadline."
                .ToList();

            return todos;
        }

        public IQueryable<GetAllTodoDto> GetTodoByUserName(int todoId)
        {
            // lets try query syntax
            var query = from td in _context.Todos where td.Id == todoId select new GetAllTodoDto {
                    Id = td.Id,
                    TaskName = td.TaskName,
                    UserName = td.User.Name,  // user cannot be null becase it is checked in CreateTodo function
                    IsComplete = td.IsComplete,
                    DeadLine = td.DeadLine,
                    UserId = td.UserId
            };

            return query;
        }

        public bool TodoExist(int todoId)
        {
            return _context.Todos.Any(u => u.Id == todoId);
        }
    }
}