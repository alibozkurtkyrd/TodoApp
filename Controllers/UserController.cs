using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Data.Abstract;
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
        private readonly IUserRepository _userRepository;

        public UserController (ApiDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {

            var userGetAllDto = _userRepository.GetAllUsersWithTodo();
            return Ok(userGetAllDto);
        }
        [HttpGet("{UserId}")]
        public IActionResult GetUser(int UserId)
        {
            if(!_userRepository.UserExist(UserId))
                return NotFound();

            var user = _userRepository.GetById(UserId);

            if (!ModelState.IsValid)
               return BadRequest(ModelState);

            return Ok(
                new {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                }
            );
        }

        [HttpPost]
        [Route("login")]
        public IActionResult userLogin(User user)
        {
          
             if (!_userRepository.UserExist(user))
                return NotFound();


            var userLoginDto = _userRepository.Login(user);

           
        
            return Ok(userLoginDto);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Regiteration(User user)
        {

            var dbUser = _context.Users.Where(u =>u.Email == user.Email).FirstOrDefault(); // Email unique degere sahip
            
            if (dbUser != null){
                return BadRequest("User already exists");
            }	

            user.Password = PasswordEncription.hashPassword(user.Password); // database e pasaportu hash edilmiş sekilde gönderizyoruz
            _userRepository.Register(user);
            
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