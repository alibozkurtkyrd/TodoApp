using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Dto;
using TodoApp.Model;

namespace TodoApp.Data.Abstract
{
    public interface ITodoRepository: IRepository<Todo>
    {
        IQueryable<GetAllTodoDto> GetTodoByUserName(int todoId);

        List<GetAllTodoDto> GetAllTodosWithUser();

        bool TodoExist(int todoId);
    }
}