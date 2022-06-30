using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Tools
{
    public class PasswordEncription
    {
        // bu methodu kullancını girmiş oldugu deger database e hash edilmiş sekilde kaydedilsin diye olusturum.
        // Key kullanma gibi komplex bir mekanizma yok 
        public static string hashPassword(string password)
        {
            var sha = SHA256.Create(); // this is for initilization
            var asByteArray = Encoding.Default.GetBytes(password); // commputeHash fonksiyonu byte array olarak bir argüman istiyor
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword); // hash code string e ceviriyoruz
        }
    }
}