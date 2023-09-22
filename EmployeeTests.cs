using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestDataGenerationWithAutoFixture
{
    [TestClass]
    public class EmployeeTests
    {
        private readonly IFixture _fixture;

        public EmployeeTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            }); ;

        }

        [Fact]
        [TestMethod]
        public void WhenConstrcutorIsInvoked_ThenValidInstanceIsReturned()
        {
            // Arrange
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var age = _fixture.Create<int>();
            var salary = _fixture.Create<decimal>();

            //Act
            var employee = new Employee(firstName, lastName, age, salary);

            //Assert
            employee.FirstName.Should().Be(firstName);
            employee.LastName.Should().Be(lastName);
            employee.Age.Should().Be(age);
            employee.Salary.Should().Be(salary);
        }

        [Fact]
        [TestMethod]
        public void WhenAddEmployeeIsInvoked_ThenEmployeeIsAddedToTheList()
        {
            //Arrange
            var employee = _fixture.Create<Employee>();
            var department = _fixture.Create<Department>();
            var employeeCount = department.Employees.Count();

            //Act
            department.AddEmployee(employee);


            //Assert 
            department.Employees.Count().Should().Be(employeeCount + 1);
        }

        [Fact]
        [TestMethod]
        public void Custom_Test_WhenAddEmployeeIsInvoked_ThenEmployeeIsAddedToTheList()
        {
            // Arrange
            var employee = _fixture.Create<Employee>();
            var employees = _fixture.CreateMany<Employee>(5).ToList();
            var department = _fixture.Build<Department>().With(x => x.Employees, employees).Create();

            //Act
            department.AddEmployee(employee);

            //Assert
            department.Employees.Count.Should().Be(6);
        }

        
        [TestMethod]
        [Theory, AutoData]
        public void WhenConstructorIsInvoked_ThenValidInstanceIsReturned( string firstName, string lastName, int age, decimal salary)
        {
            // Act
            var employee = new Employee(firstName, lastName, age, salary);

            // Assert
            employee.FirstName.Should().Be(firstName);
            employee.LastName.Should().Be(lastName);
            employee.Age.Should().Be(age);
            employee.Salary.Should().Be(salary);
        }

        //Omit auto properties
        [Fact]
        [TestMethod]
        public void Omit_AutoProperties_WhenAddEmployeeIsInvoked_ThenEmployeeIsAddedToTheList()
        {
            // Arrange
            var employee = _fixture.Build<Employee>()
                .OmitAutoProperties()
                .Create();

            var employees = _fixture.Build<Employee>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();

            var department = _fixture.Build<Department>()
                .With(x => x.Employees, employees)
                .Create();

            // Act
            department.AddEmployee(employee);

            // Assert
            department.Employees.Count.Should().Be(6);
        }

        // use OmitAutoProperties() but still initialize some of the properties
        [Theory, AutoData]
        [TestMethod]
        public void GivenEmployeeExists_WhenGetEmployeeIsInvoked_ThenEmployeeIsReturned( string firstName, string lastName)
        {
            // Arrange
            var employees = _fixture.Build<Employee>()
                .OmitAutoProperties()
                .With(x => x.FirstName, firstName)
                .With(x => x.LastName, lastName)
                .CreateMany(1)
                .ToList();
            var department = _fixture.Build<Department>()
                .With(x => x.Employees, employees)
                .Create();
            // Act
            var employee = department.GetEmployee(firstName);
            // Assert 
            employee.Should().NotBeNull();
            employee.LastName.Should().Be(lastName);
            //employee.Age.Should().BeNull();
        }

        [Fact]
        [TestMethod]
        public void WhenCalculateAverageSalaryIsInvoked_ThenAccurateResultIsReturned()
        {
            // Arrange
            var employees = _fixture.Build<Employee>()
                .WithAutoProperties()
                .CreateMany(5)
                .ToList();

            var department = _fixture.Build<Department>()
                .With(x => x.Employees, employees)
                .Create();

            // Act
            var averageSalary = department.CalculateAverageSalary();

            // Assert
            averageSalary.Should().BeGreaterThan(0);
            department.Manager.Should().NotBeNull();
            department.Manager.Name.Should().NotBeNull();
        }
    }
}
