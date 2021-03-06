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

        public User FindUser(int userId)
        { // bu fonksiyon yeni bir Todo listesi eklenirken Todo.User sutununa user eklemek için kullanılcack
            return _context.Users.Find(userId);
        }

        public List<GetAllTodoDto> GetAllTodosWithUser()
        { // Todo bilgileri arasında UserName bilgisinide getimiş olur
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
                    UserId =  t1.UserId               
                })
                .OrderBy(t1 => t1.IsComplete) 
                .ThenBy(t1 => t1.DeadLine)   // The todo list sorting is done according to the "task completion status" and secondly according to the "deadline."
                .ToList();

            return todos;
        }

        public List<getTodoDto> GetTodoByUserId(int userId)
        { // userId'ye göre Todo'yu bulmamızı sağlar

            var user =  _context.Users.Find(userId); // first find the user

            var Dtos =  _context.Todos
                .Where(t => t.UserId == userId)
                .Select(t1 => new getTodoDto
                    {
                        Id = t1.Id,
                        TaskName = t1.TaskName,
                        UserName = user.Name,  
                        IsComplete = t1.IsComplete,
                        DeadLine = t1.DeadLine,
                    }

                )
                .ToList();// thanks to getTodoDto class, we can show the username which is not belongs to Todo.cs class

            return Dtos;
        }

        public IQueryable<GetAllTodoDto> GetTodoWithUserName(int todoId)
        { // Tüm Todo'ları listeler. Bu method sayesinde Username bilgisinide görmüş oluyoruz
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
            return _context.Todos.Any(t => t.Id == todoId);
        }

        public bool UserExist(int userId)
        { // this function is require for getTodoByUserId in TodoControllers.c file
            return _context.Users.Any(u => u.Id == userId);
        }

        
    }
}