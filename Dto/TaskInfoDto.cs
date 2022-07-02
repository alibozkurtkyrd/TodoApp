using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Dto
{
    public class TaskInfoDto
    {
        // bu sınıf users verilerinini tümünü getirmede kullanılcack
        // todo nun id'si ve taskName i basmış olacagım

        public int TodoId { get; set; }
        public string TaskName { get; set; }
    }
}