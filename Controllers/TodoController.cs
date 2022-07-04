using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Data.Abstract;
using TodoApp.Dto;
using TodoApp.Model;


namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly ITodoRepository _todoRepository;

        public TodoController ( ApiDbContext context, ITodoRepository todoRepository )
        {
            _context = context;
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public IActionResult GetTodos()
        {
            var todos = _todoRepository.GetAllTodosWithUser();

            return Ok(todos);
        }

        [HttpGet("todoid/{todoid}")]
        public IActionResult GetTodo(int todoId)
        { 

            if (!_todoRepository.TodoExist(todoId))
                return NotFound();

            var query = _todoRepository.GetTodoWithUserName(todoId);
         
            return Ok(query);
        }

        [HttpGet("userid/{userId}")]
        public ActionResult<List<getTodoDto>> getTodoByUserId(int userId)
        {
            if(!_todoRepository.UserExist(userId))
                return NotFound();
                
            var Dtos = _todoRepository.GetTodoByUserId(userId);
          
            return Ok(Dtos);

        }

        // [HttpPost]
        // public async Task<ActionResult<List<getTodoDto>>> CreateTodo (createTodoDto item)
        // {
        //     var user = await _context.Users.FindAsync(item.UserId); // ilgili user ı bluyoruz

        //     if (user == null) // aslında user id tanımlanmasada olur
        //         return NotFound();


        //     var newTodo = new Todo
        //     {
        //         TaskName = item.TaskName,
        //         IsComplete = item.IsComplete,
        //         DeadLine = new DateTime(item.Year, item.Month, item.Day), 
        //         User = user,
        //         UserId = item.UserId
        //     };

        //      _context.Todos.Add(newTodo);
        //     await _context.SaveChangesAsync();
        //     return await getTodoByUserId(newTodo.UserId);
        // }


        // [HttpPut("{id}")]

        // public async Task<IActionResult> UpdateTodo(int id, UpdateTodoDto item)
        // {
        //     if(id != item.Id) // 
        //         return BadRequest();

        //     var existTodo = await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);

        //     if (existTodo == null) // bu durumda database'de id li bir todo yoktur
        //         return NotFound();

        //     var user = await _context.Users.FindAsync(item.UserId); 

        //     if (user == null) // kullnaıcnı girmiş oldugu user id hatalı (databasede olan bir userid girilmeli)
        //         return BadRequest("Userid is incorrect that's not found in database");
        //     // proplem ancak bu durumda null degere sahip olan Todo'ları güncelleyemiyoruz

        //     existTodo.TaskName = item.TaskName;
        //     existTodo.IsComplete = item.IsComplete;
        //     existTodo.DeadLine = new DateTime(item.Year, item.Month, item.Day); 
        //     existTodo.User =user;
        //     existTodo.UserId = item.UserId;
            
 
        //     await _context.SaveChangesAsync();

        //     return await GetTodo(id);
        // }

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