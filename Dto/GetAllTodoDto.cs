using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Dto
{
    public class GetAllTodoDto
    {
         public int Id {get; set;}
        public string TaskName { get; set; }
       
        public string? UserName {get; set;}
        public bool IsComplete { get; set; }

        public DateTime? DeadLine { get; set; }

        public int UserId { get; set; }
    }
}