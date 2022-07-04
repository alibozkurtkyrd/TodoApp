using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Model;

namespace TodoApp.Data.Abstract
{
    public interface ITodoRepository: IRepository<Todo>
    {
        Todo GetTodoByUserId(int userId);
    }
}