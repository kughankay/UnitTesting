using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerationWithAutoFixture
{
    public class Department
    {
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }

        public Manager Manager { get; set; }

        private readonly IPayrollService _payrollService;

        public Department(string name, Manager manager, IPayrollService payRollService)
        {
            Name = name;
            Manager = manager;
            _payrollService = payRollService;
            Employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }
        public Employee GetEmployee(string firstName)
        {
            return Employees
                .FirstOrDefault(e => e.FirstName == firstName);
        }
        public decimal CalculateAverageSalary()
        {
            if (!Employees.Any())
            {
                return 0;
            }
            return Employees.Average(e => e.Salary);
        }


    }
}
