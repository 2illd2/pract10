using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    class Cashier
    {
        private List<Product> products;
        private Dictionary<Product, int> checkout; 
        private List<FinancialRecord> financialRecords;


        public Cashier()
        {
            products = SerializationHelper.Deserialize<List<Product>>("products.json") ?? new List<Product>();
            checkout = new Dictionary<Product, int>();
            financialRecords = SerializationHelper.Deserialize<List<FinancialRecord>>("financialRecords.json") ?? new List<FinancialRecord>();
        }

        public void ShowMenu()
        {
            ConsoleKeyInfo keyInfo;
            int selectedProductIndex = 0; 

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите товары для покупки:");
                for (int i = 0; i < products.Count; i++)
                {
                    if (i == selectedProductIndex)
                    {
                        Console.Write("-> "); 
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    Console.WriteLine($"{products[i].Name} - {products[i].PricePerUnit} (в наличии: {products[i].QuantityInStock})");
                }

                keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedProductIndex = (selectedProductIndex > 0) ? selectedProductIndex - 1 : products.Count - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedProductIndex = (selectedProductIndex < products.Count - 1) ? selectedProductIndex + 1 : 0;
                        break;
                    case ConsoleKey.Add:
                    case ConsoleKey.OemPlus:
                        AddProductToCheckout(products[selectedProductIndex]);
                        break;
                    case ConsoleKey.Subtract:
                    case ConsoleKey.OemMinus:
                        RemoveProductFromCheckout(products[selectedProductIndex]);
                        break;
                    case ConsoleKey.S:
                        CompleteOrder();
                        return; 
                    case ConsoleKey.Escape:
                        return; 
                }
            }
        }

        private void AddProductToCheckout(Product product)
        {
            if (product.QuantityInStock > 0 && (!checkout.ContainsKey(product) || checkout[product] < product.QuantityInStock))
            {
                if (checkout.ContainsKey(product))
                {
                    checkout[product]++;
                }
                else
                {
                    checkout.Add(product, 1);
                }
                product.QuantityInStock--; 
                Console.WriteLine($"Товар {product.Name} добавлен в чек. Текущее количество: {checkout[product]}");
            }
            else
            {
                Console.WriteLine($"Невозможно добавить товар {product.Name}. Недостаточно товара на складе.");
            }
            Console.ReadKey();
        }

        private void RemoveProductFromCheckout(Product product)
        {
            if (checkout.ContainsKey(product) && checkout[product] > 0)
            {
                checkout[product]--;
                product.QuantityInStock++; 
                if (checkout[product] == 0)
                {
                    checkout.Remove(product);
                }
                Console.WriteLine($"Товар {product.Name} удален из чека. Текущее количество: {checkout[product]}");
            }
            else
            {
                Console.WriteLine($"Товар {product.Name} не находится в чеке.");
            }
            Console.ReadKey();
        }

        private void CompleteOrder()
        {
            decimal totalSum = 0;
            var a = "ss";
            foreach (var item in checkout)
            {
                totalSum += item.Key.PricePerUnit * item.Value;
                a = item.Key.Name;
                Console.WriteLine($"{item.Key.Name} x {item.Value} = {item.Key.PricePerUnit * item.Value}");
            }
            Console.WriteLine($"Общая сумма заказа: {totalSum}");

            SerializationHelper.Serialize(products, "products.json"); 

            var financialRecord = new FinancialRecord
            {
                ID = financialRecords.Any() ? financialRecords.Max(fr => fr.ID) + 1 : 1,
                Name = a,
                Amount = totalSum,
                Date = DateTime.Now,
                Type = "Приход"
            };
        
            financialRecords.Add(financialRecord);
            SerializationHelper.Serialize(financialRecords, "financialRecords.json");

            checkout.Clear(); 
            Console.WriteLine("Заказ завершен. Нажмите любую клавишу для возврата в меню.");
            Console.ReadKey();
        }
    }


}
