using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Dto;
using TodoApp.Model;
using TodoApp.Tools;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly ApiDbContext _context;

        public UserController (ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {

            
            // var users = await _context.Users 
            //     .Select(u => new UserGetAllDTo
            //     {
            //         Id = u.Id,
            //         FullName = String.Concat(u.Name, " ", u.Surname),
            //         Email = u.Email,
            //         Todos = u.Todos
            //     })
            //      // The todo list sorting is done according to the "task completion status" and secondly according to the "deadline."
            //     .ToListAsync();

            List<UserGetAllDTo> dtos = new List<UserGetAllDTo>();
            // List taskInfo = new List;
             
             
            // query syntax
            var users = from user in _context.Users select new UserGetAllDTo
            {
                 Id = user.Id,
                 FullName = String.Concat(user.Name, " ", user.Surname),
                 Email = user.Email,
                 Todos = user.Todos
            };

            foreach (var user in users)
            {      
                // List<String> taskInfo = new List<string>();
                // ArrayList taskInfo = new ArrayList(); // for every iteration it should be redefined for getting empty list

                List<TaskInfoDto> taskInfo = new List<TaskInfoDto>();
               
                if (user.Todos != null)
                {
                    // Console.WriteLine("if İÇERİSİ ");
                    // Console.WriteLine($"{user.Todos.First().Id}");
                    foreach (var item in user.Todos) // içte olan foreach loop unu kullanma nedenm:
                    // user bilgilerini listelerken todo ile ilgili sadece todo id ve todo taskname i kullanmak istememdir
                    //yorum satırındkai method query todo bilgerini getiriyordu ancak hepsini getirdigi için kulanmak istemedim
                    {
                        // Console.WriteLine($"{item.TaskName}");
                        // taskInfo.Add(item.Id);
                        // taskInfo.Add(item.TaskName);
                        taskInfo.Add(new TaskInfoDto {
                            TodoId = item.Id,
                            TaskName = item.TaskName
                        });
                        // Console.WriteLine($"{taskInfo}");
                    }
                }


                dtos.Add( new UserGetAllDTo {
                    Id = user.Id,
                    FullName = user.FullName, //String.Concat(user.Name, " ", user.Surname),
                    Email = user.Email,
                    TaskInfo = taskInfo,
                });
                //taskInfo.Clear(); // for every iteration all elements should be removed

            }
            return Ok(dtos);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin(User user)
        {
            string password = PasswordEncription.hashPassword(user.Password); // PasswordEncriptio Tools klasörü altında olusturulan dosya
            var dbUser = await _context.Users.Where(u => u.Email == user.Email && u.Password == password).FirstOrDefaultAsync();

            if (dbUser == null)
            {
                return BadRequest("Username or password is incorrect");
            }


            var getTodoDtos = await _context.Todos // this line is required for showing todos collection property
                .Where(t => t.UserId == dbUser.Id)
                .Select(t1 => new getTodoDto { // I do not want to get "UserId" from Todo table thus, I use select clauses
                    Id = t1.Id,
                    TaskName = t1.TaskName,
                    IsComplete = t1.IsComplete,
                    DeadLine = t1.DeadLine,
                    UserName = dbUser.Name,
                }).ToListAsync(); // get the list of todos list belogns to user from Todos table      
                

            return Ok(new UserLoginDto
            {
                Id = dbUser.Id,
                Name = dbUser.Name,
                Surname = dbUser.Surname,
                PhoneNumber = dbUser.PhoneNumber,
                Email = dbUser.Email,
                getTodoDtos = getTodoDtos
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Regiteration(User user)
        {

            var dbUser = await _context.Users.Where(u =>u.Email == user.Email).FirstOrDefaultAsync(); // Email unique degere sahip
            
            if (dbUser != null){
                return BadRequest("Username already exists");
            }	

            user.Password = PasswordEncription.hashPassword(user.Password); // database e pasaportu hash edilmiş sekilde gönderizyoruz
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok("User is succesfully registired");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if(id != user.Id) // json'da id degerini yazmayı unutma
                return BadRequest();

            var existUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existUser == null) // user yok
                return NotFound();


            existUser.Name = user.Name;
            existUser.Surname = user.Surname;
            existUser.PhoneNumber = user.PhoneNumber;
            existUser.Email = user.Email;
            existUser.Password = PasswordEncription.hashPassword(user.Password);
            // NOT: Todo'ları update işlemi buradan yapılmıyor

            await _context.SaveChangesAsync();

            return Ok("User is succesfully updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existUser == null)
                return NotFound();

            _context.Users.Remove(existUser);
            await _context.SaveChangesAsync();

            return Ok("Delete operation is Succesful");
        }



    }
}