using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public int QuantityInStock { get; set; }

        public Product(int id, string name, decimal pricePerUnit, int quantityInStock)
        {
            ID = id;
            Name = name;
            PricePerUnit = pricePerUnit;
            QuantityInStock = quantityInStock;
        }
    }

}
