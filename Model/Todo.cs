using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TodoApp.Model
{
    public class Todo
    {
        
        public int Id { get; set; }
        public string TaskName { get; set; } = string.Empty;

        public bool IsComplete { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")] // bu calısmayabilir api'de çalısıtıgımız icin
        public DateTime? DeadLine { get; set; }   

        public int UserId { get; set; } // this is for 1 to n relation
        
        [JsonIgnore]
        // simple navigation property
        public User User { get; set; }
    }
}