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
                    Id = t1.Id,
                    TaskName = t1.TaskName,
                    UserName = t1.User.Name,  // user cannot be null becase it is checked in CreateTodo function
                    IsComplete = t1.IsComplete,
                    DeadLine = t1.DeadLine,
                    UserId = t1.UserId
                })
                .OrderBy(t1 => t1.IsComplete) 
                .ThenBy(t1 => t1.DeadLine)   // The todo list sorting is done according to the "task completion status" and secondly according to the "deadline."
                .ToListAsync();

            return Ok(todos);
        }

        [HttpGet("todoid/{todoid}")]
        public async Task<IActionResult> GetTodo(int todoId)
        { // bu method çıkarılmalı bence ya da 
            //var todo = await _context.Todos.FindAsync(todoId);

            // lets try query syntax
            var query = from td in _context.Todos where td.Id == todoId select new {
                    Id = td.Id,
                    TaskName = td.TaskName,
                    UserName = td.User.Name,  // user cannot be null becase it is checked in CreateTodo function
                    IsComplete = td.IsComplete,
                    DeadLine = td.DeadLine,
                    UserId = td.UserId
            };

            return Ok(query);
        }

        [HttpGet("userid/{userId}")]
        public async Task<ActionResult<List<getTodoDto>>> getTodoByUserId(int userId)
        {
             var user = await _context.Users.FindAsync(userId); // first find the user
             var Dtos = await _context.Todos
                .Where(t => t.UserId == userId)
                .Select(t1 => new getTodoDto
                    {
                        Id = t1.Id,
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


        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTodo(int id, UpdateTodoDto item)
        {
            if(id != item.Id) // 
                return BadRequest();

            var existTodo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (existTodo == null) // bu durumda database'de id li bir todo yoktur
                return NotFound();

            var user = await _context.Users.FindAsync(item.UserId); 

            if (user == null) // kullnaıcnı girmiş oldugu user id hatalı (database olan bir userid girilmeli)
                return BadRequest("Userid is incorrect that's not found in database");


            existTodo.TaskName = item.TaskName;
            existTodo.IsComplete = item.IsComplete;
            existTodo.DeadLine = new DateTime(item.Year, item.Month, item.Day); 
            existTodo.User =user;
            existTodo.UserId = item.UserId;
            
 
            await _context.SaveChangesAsync();

            return await GetTodo(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var existTodo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (existTodo == null)
                return NotFound();

            _context.Todos.Remove(existTodo);
            await _context.SaveChangesAsync();

            return Ok(existTodo);
        }
     
    }
}