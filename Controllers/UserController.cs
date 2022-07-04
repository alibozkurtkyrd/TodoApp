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
        
        
        private readonly IUserRepository _userRepository;

        

        public UserController ( IUserRepository userRepository, ITodoRepository todoRepository)
        {
            
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
            if (user == null)
                return BadRequest(ModelState);

            if (_userRepository.UserExist(user))
                return BadRequest("User already exists");

            user.Password = PasswordEncription.hashPassword(user.Password); // database e pasaportu hash edilmiş sekilde gönderizyoruz
            _userRepository.Register(user);
            
            return Ok("User is succesfully registired");
        }


        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (user == null)
                return BadRequest(ModelState);

            if(id != user.Id) // json'da id degerini yazmayı unutma
                return BadRequest("Check Id");

            if (!_userRepository.UserExist(id))
                return NotFound();


            user.Password = PasswordEncription.hashPassword(user.Password);

            _userRepository.Update(user);

            return Ok("User is succesfully updated");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {

            if(!_userRepository.UserExist(id))
            {
                return NotFound();
            }

            var userToDelete = _userRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            _userRepository.Delete(userToDelete);
           

            return Ok("Delete operation is Succesful");
        }



    }
}