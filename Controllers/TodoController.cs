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
        { // bu method çıkarılmalı bence ya da 
            var todos = await _context.Todos.ToListAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<getTodoDto>>> getTodo(int userId)
        {
              var user = await _context.Users.FindAsync(userId);
             var Dtos = await _context.Todos
                .Where(t => t.UserId == userId)
                .Select(t1 => new getTodoDto
                    {
                        TaskName = t1.TaskName,
                         UserName = user.Name,
                        IsComplete = t1.IsComplete,
                        DeadLine = t1.DeadLine,
                    }

                )
                .ToListAsync();
                // thanks to getTodoDto class, we can show the username which is not belongs to Todo.cs class

          

            // var todos = await _context.Todos.Where(t => t.UserId == userId).ToListAsync();

            // var todos = from todo in _context.Todos
            //     select new getTodoDto
            //     {

            //     }
            // List<getTodoDto> Dtos  = new List<getTodoDto>();
            // foreach (var todo in todos)
            // {                
            //     Dtos.Add(new getTodoDto {
            //         TaskName = todo.TaskName,
            //         UserName = user.Name,
            //         IsComplete = todo.IsComplete,
            //         DeadLine = todo.DeadLine,
                    
            //     });
            // }

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


            // var getTodo = new getTodoDto
            // {
            //     TaskName = item.TaskName,
            //     UserName = user.Name,
            //     IsComplete = item.IsComplete,
            //     DeadLine = new DateTime(item.Year, item.Month, item.Day), 
                
            // };

             _context.Todos.Add(newTodo);
            await _context.SaveChangesAsync();
            return await getTodo(newTodo.UserId);
        }
        /*
        [HttpPost]
        public async Task<IActionResult> CreateTodo(createTodoDto todo)
        { 
            if (ModelState.IsValid)// buraya daha anlamlı bir kosul olabilir (null mu diye bakabilirsin)
            {
                await _context.Todos.AddAsync(todo);
                await _context.SaveChangesAsync();

                // sanırsam buralara user in list prop degerine ekleme yapılmaıs gerekebilr emin degilim örnek gerek
                return CreatedAtAction("GetItem", new {todo.Id}, todo); // asagıya GetItem methodu yazmlasın
            }

            return new JsonResult("Something went wrong") {StatusCode = 500};
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            var item = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        */
    }
}