using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1_1
{
    class Employee
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PassportSeriesNumber { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public int UserID { get; set; }

        public Employee(int id, string lastName, string firstName, string middleName,
                DateTime birthDate, string passportSeriesNumber, string position,
                decimal salary, int userId)

        {
            ID = id;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            BirthDate = birthDate;
            PassportSeriesNumber = passportSeriesNumber;
            Position = position;
            Salary = salary;
            UserID = userId;
        }
    }

}
