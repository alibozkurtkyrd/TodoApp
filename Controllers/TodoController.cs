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
        
        private readonly ITodoRepository _todoRepository;

        public TodoController (ITodoRepository todoRepository )
        {
            
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

        [HttpPost]
        public IActionResult CreateTodo (createTodoDto item)
        {

            var user = _todoRepository.FindUser(item.UserId);

            
            var newTodo = new Todo
            {
                TaskName = item.TaskName,
                IsComplete = item.IsComplete,
                DeadLine = new DateTime(item.Year, item.Month, item.Day), 
                User =  user,
                UserId = (user == null ? null : user.Id)
            };

            _todoRepository.Create(newTodo);

            return GetTodo(newTodo.Id);
        }


        [HttpPut("{id}")]

        public IActionResult UpdateTodo(int id, UpdateTodoDto item)
        {
            if (item == null)
                return BadRequest(ModelState);

            if(id != item.Id) // json'da id degerini yazmayı unutma
                return BadRequest("Check Id");

            if (!_todoRepository.TodoExist(id))
                return NotFound();

            var existTodo = _todoRepository.GetById(id);
            var user =  _todoRepository.FindUser(item.UserId); //user id degismesi durumuna karşı yeni user bulunmalı
                // user olmayadabilir

            existTodo.TaskName = item.TaskName;
            existTodo.IsComplete = item.IsComplete;
            existTodo.DeadLine = new DateTime(item.Year, item.Month, item.Day); 
            existTodo.User =user; // user null olabilir
            existTodo.UserId = (user == null ? null : item.UserId); // eger user null ise userId null olarak atancak 
                                                        // user null degil ise item.UserId(user.Id) atanacak
            
            
            _todoRepository.Update(existTodo);

            return  GetTodo(id);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            if(!_todoRepository.TodoExist(id))
            {
                return NotFound();
            }

            var todoDelete = _todoRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            _todoRepository.Delete(todoDelete);
           

            return Ok("Delete operation is Succesful");

        }
     
    }
}