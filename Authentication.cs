using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Authentication
    {
        private List<User> users;
        private List<Employee> employees; 

        public Authentication(string usersFilePath, string employeesFilePath)
        {
            users = SerializationHelper.Deserialize<List<User>>(usersFilePath) ?? new List<User>();
            employees = SerializationHelper.Deserialize<List<Employee>>(employeesFilePath) ?? new List<Employee>();
            foreach (var u in users)
            {
                Console.WriteLine($"ID: {u.ID}, Login: {u.Login}, Password: {u.Password}, Role: {u.Role}");
            }
            foreach (var u in employees)
            {
                Console.WriteLine($"ID: {u.ID}, Name: {u.LastName} UserID: {u.UserID}");
            }
        }

        public User Login()
        {
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = ReadPassword();

            var user = users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user != null)
            {
                var employee = employees.FirstOrDefault(e => e.UserID == user.ID);
                string displayName = employee != null ? $"{employee.FirstName} {employee.LastName}" : user.Login;
                Console.WriteLine($"Добро пожаловать, {displayName}!");
                return user;
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль.");
                return null;
            }
        }

 

    private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (!string.IsNullOrEmpty(password))
                {
                    password = password.Substring(0, password.Length - 1);
                    int pos = Console.CursorLeft;
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password;
        }
    }

}
