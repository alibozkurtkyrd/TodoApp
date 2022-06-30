using System;
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


            var getTodoDtos = await _context.Todos
                .Where(t => t.UserId == dbUser.Id)
                .Select(t1 => new getTodoDto { // I do not want to get "UserId" from Todo table thus, I use select clauses
                    Id = t1.Id,
                    TaskName = t1.TaskName,
                    IsComplete = t1.IsComplete,
                    DeadLine = t1.DeadLine,
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

            var dbUser = await _context.Users.Where(u =>u.Name == user.Name).FirstOrDefaultAsync(); // daha sonradan bunu maile göre yaparsan daha iyi olur
            // email unique icin https://stackoverflow.com/questions/41246614/entity-framework-core-add-unique-constraint-code-first
            
            if (dbUser != null){
                return BadRequest("Username already exists");
            }	

            user.Password = PasswordEncription.hashPassword(user.Password); // database e pasaportu hash edilmiş sekilde gönderizyoruz
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok("User is succesfully registired");
        }


    }
}