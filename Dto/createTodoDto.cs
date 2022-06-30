using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Dto
{
    public class createTodoDto
    {
        public string TaskName { get; set; } = "give name";

        public bool IsComplete { get; set; } = false;

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }
        
        public int UserId { get; set; } // this is for 1 to n relation

    }
}