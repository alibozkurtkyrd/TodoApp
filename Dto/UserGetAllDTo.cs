using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TodoApp.Model;

namespace TodoApp.Dto
{
    public class UserGetAllDTo
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; } 

        [JsonIgnore]
        public ICollection<Todo>? Todos { get; set; }

        // public ArrayList? TaskInfo { get; set; } // it stores the taskName and taskId

        public List<TaskInfoDto> TaskInfo { get; set; }

        // public List<string>? TaskInfo { get; set; } // it stores the taskName and taskId
        // public int TaskId { get; set; }

        // public string TaskName { get; set; }
    }
}