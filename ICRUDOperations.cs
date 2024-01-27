using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    interface ICRUDOperations
    {
        void Create();
        void Read();
        void Update();
        void Delete();
        void Search();
    }

}
