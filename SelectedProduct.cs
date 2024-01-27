using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    class SelectedProduct : Product
    {
       

        public SelectedProduct(int id, string name, decimal pricePerUnit, int quantityInStock)
            : base(id, name, pricePerUnit, quantityInStock)
        {
        }
    }

}
