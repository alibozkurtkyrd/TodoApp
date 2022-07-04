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
        IQueryable<GetAllTodoDto> GetTodoWithUserName(int todoId);

        List<GetAllTodoDto> GetAllTodosWithUser();

        List<getTodoDto> GetTodoByUserId(int userId);

        bool TodoExist(int todoId);

        bool UserExist(int userId);
    }
}