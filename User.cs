using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public User(int id, string login, string password, UserRole role)
        {
            ID = id;
            Login = login;
            Password = password;
            Role = role;
        }
    }

}
