using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data.Abstract;
using TodoApp.Dto;
using TodoApp.Model;
using TodoApp.Tools;

namespace TodoApp.Data.Concrete
{
    public class UserRepository : 
    GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApiDbContext context) :base(context)
        {
            
        }

        public List<UserGetAllDTo> GetAllUsersWithTodo()
        {
                     // var users = await _context.Users 
            //     .Select(u => new UserGetAllDTo
            //     {
            //         Id = u.Id,
            //         FullName = String.Concat(u.Name, " ", u.Surname),
            //         Email = u.Email,
            //         Todos = u.Todos
            //     })
            //      // The todo list sorting is done according to the "task completion status" and secondly according to the "deadline."
            //     .ToListAsync();

            List<UserGetAllDTo> dtos = new List<UserGetAllDTo>();
                     
            //query syntax
            var users = from user in _context.Users select new UserGetAllDTo
            {
                 Id = user.Id,
                 FullName = String.Concat(user.Name, " ", user.Surname),
                 Email = user.Email,
                 Todos = user.Todos
            };

            foreach (var user in users)
            {      
                
                List<TaskInfoDto> taskInfo = new List<TaskInfoDto>(); // her bir döngüde yeniden tanımlamış oluyorum
                // dışarıda tanımlayınca Clear() methodu hata veriyor
                if (user.Todos != null)
                {
                    // Console.WriteLine("if İÇERİSİ ");
                    // Console.WriteLine($"{user.Todos.First().Id}");
                    foreach (var item in user.Todos) // içte olan foreach loop unu kullanma nedenm:
                    // user bilgilerini listelerken todo ile ilgili sadece todo id ve todo taskname i kullanmak istememdir
                    //yorum satırındkai method query todo bilgerini getiriyordu ancak hepsini getirdigi için kulanmak istemedim
                    {
                        
                        taskInfo.Add(new TaskInfoDto {
                            TodoId = item.Id,
                            TaskName = item.TaskName
                        });
                        
                    }
                }


                dtos.Add( new UserGetAllDTo {
                    Id = user.Id,
                    FullName = user.FullName, //String.Concat(user.Name, " ", user.Surname),
                    Email = user.Email,
                    TaskInfo = taskInfo,
                });
                //taskInfo.Clear(); // for every iteration all elements should be removed

            }
            return dtos;
        }

        public UserLoginDto Login(User user)
        {

                string password = PasswordEncription.hashPassword(user.Password); // PasswordEncriptio Tools klasörü altında olusturulan dosya
                var dbUser =  _context.Users.Where(u => u.Email == user.Email && u.Password == password).FirstOrDefault();
         
                var getTodoDtos = _context.Todos // this line is required for showing todos collection property
                .Where(t => t.UserId == dbUser.Id) // null olma durumu userController içerisinde userLogin methodunda kontrol ediliyor
                .Select(t1 => new getTodoDto { // I do not want to get "UserId" from Todo table thus, I use select clauses
                    Id = t1.Id,
                    TaskName = t1.TaskName,
                    IsComplete = t1.IsComplete,
                    DeadLine = t1.DeadLine,
                    UserName = dbUser.Name,
                }).ToList(); // get the list of todos list belogns to user from Todos table      
                

            return (new UserLoginDto
                    {
                        Id = dbUser.Id,
                        Name = dbUser.Name,
                        Surname = dbUser.Surname,
                        PhoneNumber = dbUser.PhoneNumber,
                        Email = user.Email, // parametre olara userDto giriyoruz (sadece email ve password var) digerlerini database den alıyoruz
                        getTodoDtos = getTodoDtos
                    }
            );
                 
        }

        public void Register(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public bool UserExist(User user)
        {
            string password = PasswordEncription.hashPassword(user.Password); // PasswordEncriptio Tools klasörü altında olusturulan dosya
            var dbUser =  _context.Users.Where(u => u.Email == user.Email && u.Password == password).FirstOrDefault();

            if (dbUser == null)
            {
               
                return false;
            }

            return true;
        }

        public bool UserExist(int userId) // different signature from previous one
        {
            
            return _context.Users.Any(u => u.Id == userId);
        }

        // public bool CheckuniquenessEmail(UserGetAllDTo user)
        // { //Email adrese unique degere sahiptir. 
        
        //     var dbUser =  _context.Users.Where(u =>u.Email == user.Email).FirstOrDefault(); 
            
        //     if (dbUser != null){
        //         return false;
        //     }	

        //     return true;
        // }
    }
}