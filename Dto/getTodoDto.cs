using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Model;

namespace TodoApp.Dto
{
    public class getTodoDto
    { 
        public string TaskName { get; set; }
       
        public string? UserName {get; set;}
        public bool IsComplete { get; set; }

        public DateTime? DeadLine { get; set; }

        

    }
}