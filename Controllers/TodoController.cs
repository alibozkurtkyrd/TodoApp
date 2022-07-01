using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Dto;
using TodoApp.Model;


namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TodoController (ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {

             
            // var todos = await _context.Todos.ToListAsync();
            //left inner join
            var todos = await _context.Todos 
                .Include(t => t.User) // navigation property included
                .Select(t1 => new 
                {
                    TaskName = t1.TaskName,
                    UserName = t1.User.Name,  // user cannot be null becase it is checked in CreateTodo function
                    IsComplete = t1.IsComplete,
                    DeadLine = t1.DeadLine,
                })
                .OrderBy(t1 => t1.IsComplete) 
                .ThenBy(t1 => t1.DeadLine)   // The todo list sorting is done according to the "task completion status" and secondly according to the "deadline."
                .ToListAsync();

            return Ok(todos);
        }

        [HttpGet("todoid/{todoid}")]
        public async Task<IActionResult> GetTodo(int todoId)
        { // bu method çıkarılmalı bence ya da 
            var todo = await _context.Todos.FindAsync(todoId);
            return Ok(todo);
        }

        [HttpGet("userid/{userId}")]
        public async Task<ActionResult<List<getTodoDto>>> getTodoByUserId(int userId)
        {
             var user = await _context.Users.FindAsync(userId); // first find the user
             var Dtos = await _context.Todos
                .Where(t => t.UserId == userId)
                .Select(t1 => new getTodoDto
                    {
                        TaskName = t1.TaskName,
                         UserName = user.Name,  // user cannot be null becase it is checked in CreateTodo function
                        IsComplete = t1.IsComplete,
                        DeadLine = t1.DeadLine,
                    }

                )
                .ToListAsync();
                // thanks to getTodoDto class, we can show the username which is not belongs to Todo.cs class

          
            return Ok(Dtos);

        }

        [HttpPost]
        public async Task<ActionResult<List<getTodoDto>>> CreateTodo (createTodoDto item)
        {
            var user = await _context.Users.FindAsync(item.UserId); // ilgili user ı bluyoruz

            if (user == null)
                return NotFound();


            var newTodo = new Todo
            {
                TaskName = item.TaskName,
                IsComplete = item.IsComplete,
                DeadLine = new DateTime(item.Year, item.Month, item.Day), 
                User = user,
                UserId = item.UserId
            };

             _context.Todos.Add(newTodo);
            await _context.SaveChangesAsync();
            return await getTodoByUserId(newTodo.UserId);
        }
     
    }
}