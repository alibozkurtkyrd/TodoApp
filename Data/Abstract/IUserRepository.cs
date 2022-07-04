using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Dto;
using TodoApp.Model;

namespace TodoApp.Data.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        UserLoginDto Login(User user);

        void Register(User user);

        bool UserExist(User user);

        bool UserExist(int UserId);

        List<UserGetAllDTo> GetAllUsersWithTodo();
    }
}