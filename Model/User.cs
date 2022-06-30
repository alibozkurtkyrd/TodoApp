using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Surname { get; set; } 

        public string? PhoneNumber { get; set; }

        [Required]
        public string Password {get; set;}

        
        [EmailAddress]
        public string Email { get; set; } // ayrıca email database'de uniqu olmalı

        // Collection navigation property
        public ICollection<Todo>? Todos { get; set; } // it is required for 1 to n relation
        // burayı nullable yaptık ancak daha sonradan kontrol et
        
       
    }
}