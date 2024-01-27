using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1_1
{
    class AccountingRecord
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } 

        public AccountingRecord(int id, string name, decimal amount, DateTime date, string type) 
        {
            ID = id;
            Name = name;
            Amount = amount;
            Date = date;
            Type = type;
        }
    }


    class Accountant : ICRUDOperations
    {
        private List<AccountingRecord> accountingRecords; 
        private int selectedMenuItem = 0; 
        private string[] menuItems = { "Создать запись", "Просмотреть записи", "Обновить запись", "Удалить запись", "Поиск записей", "Выход" };
       
        public Accountant()
        {
           
            accountingRecords = new List<AccountingRecord>();
            LoadRecordsFromFile();
        }

        public void Create()
        {
            Console.Clear();
            Console.WriteLine("Создание новой записи в бухгалтерии");
            Console.Write("Введите название записи: ");
            string name = Console.ReadLine();:
            Console.Write("Введите сумму: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                AccountingRecord newRecord = new AccountingRecord(
                    GenerateRecordID(),
                    name,
                    amount,
                    DateTime.Now, 
                    "Приход" 
                );
                accountingRecords.Add(newRecord);
                SaveRecordsToFile();

                Console.WriteLine("Запись успешно создана.");
            }
            else
            {
                Console.WriteLine("Ошибка при вводе суммы.");
            }
            Console.ReadKey();
        }

        public void Read()
        {
            Console.Clear();
            Console.WriteLine("Список записей бухгалтерии:");
            foreach (var record in accountingRecords)
            {
                Console.WriteLine($"ID: {record.ID}, Название: {record.Name}, Сумма: {record.Amount:C}, Дата: {record.Date.ToShortDateString()}");
          
            }
            Console.ReadKey();
        }

        public void Update()
        {
            Console.Clear();
            Console.Write("Введите ID записи для обновления: ");
            if (int.TryParse(Console.ReadLine(), out int recordID))
            {
                var recordToUpdate = accountingRecords.Find(r => r.ID == recordID);
                if (recordToUpdate != null)
                {
                    Console.WriteLine($"Редактирование записи ID {recordToUpdate.ID}");

                    Console.Write("Введите новое название записи: ");
                    recordToUpdate.Name = Console.ReadLine();
                    Console.Write("Введите новую сумму: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                    {
                        recordToUpdate.Amount = amount;

                        SaveRecordsToFile();
                        Console.WriteLine("Запись успешно обновлена.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка при вводе суммы.");
                    }
                }
                else
                {
                    Console.WriteLine("Запись с указанным ID не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
            Console.ReadKey();
        }

   
        public void Delete()
        {
            Console.Clear();
            Console.Write("Введите ID записи для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int recordID))
            {
                var recordToDelete = accountingRecords.Find(r => r.ID == recordID);
                if (recordToDelete != null)
                {
                    accountingRecords.Remove(recordToDelete);

                    SaveRecordsToFile();
                    Console.WriteLine("Запись успешно удалена.");
                }
                else
                {
                    Console.WriteLine("Запись с указанным ID не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
            Console.ReadKey();
        }

        public void Search()
        {
            Console.Clear();
            Console.Write("Введите ключевое слово для поиска: ");
            string keyword = Console.ReadLine().ToLower();
            var foundRecords = accountingRecords.FindAll(r => r.Name.ToLower().Contains(keyword));

            if (foundRecords.Count == 0)
            {
                Console.WriteLine("Записи не найдены.");
            }
            else
            {
                Console.WriteLine("Найденные записи:");
                foreach (var record in foundRecords)
                {
                    Console.WriteLine($"ID: {record.ID}, Название: {record.Name}, Сумма: {record.Amount:C}, Дата: {record.Date.ToShortDateString()}");
                    
                }
            }
            Console.ReadKey();
        }

      
        public void ShowMenu()
        {
            bool exitMenu = false;

            while (!exitMenu)
            {
                Console.Clear();
                Console.WriteLine("Меню бухгалтера:");
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedMenuItem)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine($"{i + 1}. {menuItems[i]}");

                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedMenuItem = (selectedMenuItem - 1 + menuItems.Length) % menuItems.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedMenuItem = (selectedMenuItem + 1) % menuItems.Length;
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedMenuItem)
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
                                exitMenu = true;
                                break;
                        }
                        break;
                }
            }
        }


        private void SaveRecordsToFile()
        {
            string json = JsonConvert.SerializeObject(accountingRecords, Formatting.Indented);
            File.WriteAllText("financialRecords.json", json);
            Console.WriteLine("Записи сохранены в financialRecords.json");
        }

        private void LoadRecordsFromFile()
        {
            if (File.Exists("financialRecords.json"))
            {
                string json = File.ReadAllText("financialRecords.json");
                accountingRecords = JsonConvert.DeserializeObject<List<AccountingRecord>>(json);
                Console.WriteLine("Записи загружены из financialRecords.json");
            }
        }

        private int GenerateRecordID()
        {
            int lastID = accountingRecords.Count > 0 ? accountingRecords[accountingRecords.Count - 1].ID : 0;
            return lastID + 1;
        }
    }
}
