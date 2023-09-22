using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerationWithAutoFixture
{
    public interface IPayrollService
    {
        void PaySalaries(IEnumerable<Employee> employee);
    }
}
