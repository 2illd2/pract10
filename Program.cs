using project1_1;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string usersFilePath = "users.json";
        string employeesFilePath = "employees.json";
        Authentication auth = new Authentication(usersFilePath, employeesFilePath);


        User user = auth.Login();
        if (user != null)
        {

            switch (user.Role)
            {
                case UserRole.Administrator:
                    Administrator admin = new Administrator();
                    admin.ShowMenu();
                    break;
                case UserRole.Cashier:
                    Cashier cashier = new Cashier();
                    cashier.ShowMenu();
                    break;
                case UserRole.HRManager:
                    break;
                case UserRole.WarehouseManager:
                    break;
                case UserRole.Accountant:
                    Accountant accountant = new Accountant();
                    accountant.ShowMenu();
                    break;
                default:
                    Console.WriteLine("Неизвестная роль.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Ошибка входа в систему.");
        }
    }
}
