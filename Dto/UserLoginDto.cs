using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Model;

namespace TodoApp.Dto
{
    public class UserLoginDto
    {
       public int Id { get; set; }
        public string Name { get; set; }

        public string? Surname { get; set; } 

        public string? PhoneNumber { get; set; }
               
        [EmailAddress]
        public string Email { get; set; } // ayrıca email database'de uniqu olmalı

        // Collection navigation property
        public ICollection<getTodoDto>? getTodoDtos { get; set; } 
        
    }
}