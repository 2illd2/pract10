using project1_1;
using System;

class Administrator
{
    private List<User> users;
    private List<Employee> employees;
    private readonly string[] menuOptions = new string[] {
        "Создать нового пользователя",
        "Просмотреть список пользователей",
        "Обновить данные пользователя",
        "Удалить пользователя",
        "Поиск пользователя",
        "Обновить информацию о сотруднике"
    };

    public Administrator()
    {
        users = SerializationHelper.Deserialize<List<User>>("users.json") ?? new List<User>();
        employees = SerializationHelper.Deserialize<List<Employee>>("employees.json") ?? new List<Employee>();
    }

    public void ShowMenu()
    {
        int currentIndex = 0;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню администратора:");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == currentIndex)
                {
                    Console.WriteLine($"> {menuOptions[i]}");
                }
                else
                {
                    Console.WriteLine($"  {menuOptions[i]}");
                }
            }

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    currentIndex = (currentIndex > 0) ? currentIndex - 1 : menuOptions.Length - 1;
                    break;
                case ConsoleKey.DownArrow:
                    currentIndex = (currentIndex < menuOptions.Length - 1) ? currentIndex + 1 : 0;
                    break;
                case ConsoleKey.Enter:
                    ExecuteOption(currentIndex);
                    break;
                case ConsoleKey.Escape:
                    return; 
            }
        }
    }

    private void ExecuteOption(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0:
                Create();
                break;
            case 1:
                Read();
                break;
            case 2:
                Update();
                break;
            case 3:
                Delete();
                break;
            case 4:
                Search();
                break;
            case 5:
                AddEmployee();
                break;
        }
    }




    public void Create()
    {
        Console.Clear();
        Console.WriteLine("Создание нового пользователя:");
        Console.Write("Введите логин: ");
        string login = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();

        int id = users.Any() ? users.Max(u => u.ID) + 1 : 1;
        UserRole role = ChooseUserRole();
        var newUser = new User(id, login, password, role);
        users.Add(newUser);

        SerializationHelper.Serialize(users, "users.json");
        Console.WriteLine("Пользователь успешно создан.");
        Console.ReadKey();
    }

    public void Read()
    {
        Console.Clear();
        Console.WriteLine("Список всех пользователей:");

        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.ID}");
            Console.WriteLine($"Логин: {user.Login}");
            Console.WriteLine($"Пароль: {new string('*', user.Password.Length)}")
            Console.WriteLine($"Роль: {user.Role}");

            var foundEmployee = employees.FirstOrDefault(employee => employee.UserID == user.ID);
            if (foundEmployee != null)
            {
                Console.WriteLine("Информация о сотруднике:");
                Console.WriteLine($"Фамилия: {foundEmployee.LastName}");
                Console.WriteLine($"Имя: {foundEmployee.FirstName}");
                Console.WriteLine($"Отчество: {foundEmployee.MiddleName}");
                Console.WriteLine($"Дата рождения: {foundEmployee.BirthDate:yyyy-MM-dd}");
                Console.WriteLine($"Серия и номер паспорта: {foundEmployee.PassportSeriesNumber}");
                Console.WriteLine($"Должность: {foundEmployee.Position}");
                Console.WriteLine($"Зарплата: {foundEmployee.Salary}");
            }

            Console.WriteLine(new string('-', 30));
        }

        Console.ReadKey();
    }


    public void Update()
    {
        Console.Clear();
        Console.Write("Введите ID пользователя для обновления: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id) || !users.Any(u => u.ID == id))
        {
            Console.Write("Некорректный ID. Попробуйте снова: ");
        }

        var user = users.FirstOrDefault(u => u.ID == id);
        Console.Write("Введите новый логин: ");
        user.Login = Console.ReadLine();
        Console.Write("Введите новый пароль: ");
        user.Password = Console.ReadLine();
        user.Role = ChooseUserRole();

        SerializationHelper.Serialize(users, "users.json");
        Console.WriteLine("Данные пользователя обновлены.");
        Console.ReadKey();
    }

    public void Delete()
    {
        Console.Clear();
        Console.Write("Введите ID пользователя для удаления: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id) || !users.Any(u => u.ID == id))
        {
            Console.Write("Некорректный ID. Попробуйте снова: ");
        }

        var user = users.FirstOrDefault(u => u.ID == id);
        users.Remove(user);
        SerializationHelper.Serialize(users, "users.json");
        Console.WriteLine("Пользователь удален.");
        Console.ReadKey();
    }

    public void Search()
    {
        Console.Clear();

        string[] searchAttributes = new string[] { "ID", "Логин", "Роль" };
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите атрибут для поиска:");

            for (int i = 0; i < searchAttributes.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.Write("=> ");
                }
                else
                {
                    Console.Write("   ");
                }
                Console.WriteLine(searchAttributes[i]);
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % searchAttributes.Length;
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + searchAttributes.Length) % searchAttributes.Length;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        Console.Write($"Введите {searchAttributes[selectedIndex]} для поиска: ");
        string searchValue = Console.ReadLine();

        var foundUsers = users.Where(u => GetAttributeValue(u, searchAttributes[selectedIndex]).Contains(searchValue)).ToList();

        if (foundUsers.Count == 0)
        {
            Console.WriteLine("Пользователи не найдены.");
        }
        else
        {
            foreach (var user in foundUsers)
            {
                Console.WriteLine($"ID: {user.ID}, Логин: {user.Login}, Роль: {user.Role}");
            }
        }
        Console.ReadKey();
    }

    private string GetAttributeValue(User user, string attribute)
    {
        switch (attribute)
        {
            case "ID":
                return user.ID.ToString();
            case "Логин":
                return user.Login;
            case "Роль":
                return user.Role.ToString();
            default:
                return "";
        }
    }


    private UserRole ChooseUserRole()
    {
        Console.WriteLine("Выберите роль пользователя:");
        foreach (var role in Enum.GetValues(typeof(UserRole)))
        {
            Console.WriteLine($"{(int)role}. {role}");
        }
        UserRole selectedRole;
        while (!Enum.TryParse(Console.ReadLine(), out selectedRole) || !Enum.IsDefined(typeof(UserRole), selectedRole))
        {
            Console.WriteLine("Некорректный выбор. Попробуйте снова.");
        }
        return selectedRole;
    }

    private void AddEmployee()
    {
        Console.Clear();
        Console.WriteLine("Добавление информации о сотруднике:");

        Console.Write("Введите ID пользователя: ");
        if (!int.TryParse(Console.ReadLine(), out int userId) || !users.Any(u => u.ID == userId))
        {
            Console.WriteLine("Пользователь с таким ID не найден.");
            Console.ReadKey();
            return;
        }

        var employee = new Employee(id: userId, lastName: "Фамилия", firstName: "Имя", middleName: "Отчество",
                             birthDate: new DateTime(1990, 1, 1), passportSeriesNumber: "1234567890",
                             position: "Должность", salary: 50000, userId = userId);

        Console.Write("Введите фамилию сотрудника (или Enter для значения по умолчанию): ");
        string lastNameInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(lastNameInput))
        {
            employee.LastName = lastNameInput;
        }

        Console.Write("Введите имя сотрудника (или Enter для значения по умолчанию): ");
        string firstNameInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(firstNameInput))
        {
            employee.FirstName = firstNameInput;
        }

        Console.Write("Введите отчество сотрудника (или Enter для значения по умолчанию): ");
        string middleNameInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(middleNameInput))
        {
            employee.MiddleName = middleNameInput;
        }

        Console.Write("Введите дату рождения сотрудника (гггг-мм-дд) (или Enter для значения по умолчанию): ");
        string birthDateInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(birthDateInput))
        {
            if (DateTime.TryParse(birthDateInput, out DateTime birthDate))
            {
                employee.BirthDate = birthDate;
            }
            else
            {
                Console.WriteLine("Некорректный формат даты рождения.");
                Console.ReadKey();
                return;
            }
        }

        Console.Write("Введите серию и номер паспорта (или Enter для значения по умолчанию): ");
        string passportInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(passportInput))
        {
            employee.PassportSeriesNumber = passportInput;
        }

        Console.Write("Введите должность сотрудника (или Enter для значения по умолчанию): ");
        string positionInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(positionInput))
        {
            employee.Position = positionInput;
        }

        Console.Write("Введите зарплату сотрудника (или Enter для значения по умолчанию): ");
        string salaryInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(salaryInput))
        {
            if (decimal.TryParse(salaryInput, out decimal salary))
            {
                employee.Salary = salary;
            }
            else
            {
                Console.WriteLine("Некорректный формат зарплаты.");
                Console.ReadKey();
                return;
            }
        }

        employees.Add(employee);
        SerializationHelper.Serialize(employees, "employees.json");
        Console.WriteLine("Информация о сотруднике добавлена.");
        Console.ReadKey();
    }

}

