using CustomerAPI.Controllers;
using CustomerAPI.Dto;
using CustomerAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Customer.API.Test.UnitTests
{
    public class CustomersControllerTest : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture _dbFixture;

        public CustomersControllerTest(DatabaseFixture fixture)
        {
            _dbFixture = fixture;
        }

        [Fact]
        public async Task Get_customers_success()
        {
            var context = new CustomerContext(_dbFixture.DbOptions);

            var customerController = new CustomersController(context);
            var actionResult = await customerController.GetCustomersAsync();

            Assert.True(actionResult.Value.Count > 0);
        }

        [Fact]
        public async Task Get_customers_by_name_matches_first_name()
        {
            var name = "dav";

            var context = new CustomerContext(_dbFixture.DbOptions);

            var customerController = new CustomersController(context);
            var actionResult = await customerController.GetCustomersAsync(name);

            Assert.Contains(actionResult.Value, item => item.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }


        [Fact]
        public async Task Get_customers_by_name_matches_last_name()
        {
            var name = "unca";

            var context = new CustomerContext(_dbFixture.DbOptions);

            var customerController = new CustomersController(context);
            var actionResult = await customerController.GetCustomersAsync(name);

            Assert.Contains(actionResult.Value, item => item.LastName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task Get_customers_by_name_no_matches()
        {
            var name = "nomatchingname";

            var context = new CustomerContext(_dbFixture.DbOptions);

            var customerController = new CustomersController(context);
            var actionResult = await customerController.GetCustomersAsync(name);

            Assert.Empty(actionResult.Value);
        }

        [Fact]
        public async Task Get_customer_by_id_success()
        {
            var expectedFirstName = "David";
            var expectedLastName = "Robinson";

            var context = new CustomerContext(_dbFixture.DbOptions);

            var customerController = new CustomersController(context);
            var actionResult = await customerController.GetCustomerByIdAsync(2);

            Assert.Equal(expectedFirstName, actionResult.Value.FirstName);
            Assert.Equal(expectedLastName, actionResult.Value.LastName);
        }

        [Fact]
        public async Task Get_customer_by_id_not_found_result()
        {
            var context = new CustomerContext(_dbFixture.DbOptions);

            var customerController = new CustomersController(context);
            var actionResult = await customerController.GetCustomerByIdAsync(int.MaxValue);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task Create_customer_success()
        {
            var newCustomer = GetTestCustomer();

            var context = new CustomerContext(_dbFixture.DbOptions);

            var customerController = new CustomersController(context);

            var actionResult = (CreatedAtActionResult) await customerController.CreateCustomerAsync(newCustomer);

            var id = int.Parse(actionResult.RouteValues["id"].ToString());

            var customer = await customerController.GetCustomerByIdAsync(id);

            Assert.Equal(newCustomer.FirstName, customer.Value.FirstName);
            Assert.Equal(newCustomer.LastName, customer.Value.LastName);
            Assert.Equal(newCustomer.DateOfBirth, customer.Value.DateOfBirth);
        }

        [Fact]
        public async Task Update_customer_success()
        {
            var newCustomer = GetTestCustomer();
            var context = new CustomerContext(_dbFixture.DbOptions);
            var customerController = new CustomersController(context);
            var createResult = (CreatedAtActionResult)await customerController.CreateCustomerAsync(newCustomer);
            var id = int.Parse(createResult.RouteValues["id"].ToString());

            newCustomer.LastName = "Persons";
            var updateResult = await customerController.UpdateCustomerAsync(id, newCustomer);

            var actionResult = await customerController.GetCustomerByIdAsync(id);

            Assert.Equal(newCustomer.LastName, actionResult.Value.LastName);
        }

        [Fact]
        public async Task Delete_customer_success()
        {
            var newCustomer = GetTestCustomer();
            var context = new CustomerContext(_dbFixture.DbOptions);
            var customerController = new CustomersController(context);
            var createResult = (CreatedAtActionResult)await customerController.CreateCustomerAsync(newCustomer);
            var id = int.Parse(createResult.RouteValues["id"].ToString());

            var deleteResult = await customerController.DeleteCustomerByIdAsync(id);

            var actionResult = await customerController.GetCustomerByIdAsync(id);

            Assert.Null(actionResult.Value);
        }

        private CustomerCommandDTO GetTestCustomer()
        {
            return new CustomerCommandDTO { FirstName = "Test", LastName = "Customer", DateOfBirth = new DateTime(2000, 12, 15) };
        }
    }
}
