using System;
using Xunit;
using CustomerAPI;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using CustomerAPI.Dto;
using System.Net;
using System.Text;
using System.Linq;

namespace Customer.API.Test.FunctionalTests
{
    public class CustomerScenarios : CustomerScenarioBase, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CustomerScenarios(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task get_customers_returns_ok()
        {
            var httpResponse = await _client.GetAsync(Get.Customers());

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<IEnumerable<CustomerDTO>>(stringResponse);
            Assert.Contains(customers, p => p.FirstName == "David" && p.LastName == "Robinson");
            Assert.Contains(customers, p => p.FirstName == "Tim" && p.LastName == "Duncan");
        }

        [Fact]
        public async Task get_customers_with_name_returns_ok()
        {
            var httpResponse = await _client.GetAsync(Get.Customers("David"));

            httpResponse.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task get_customer_by_id_returns_ok()
        {
            var httpResponse = await _client.GetAsync(Get.CustomerById(1));

            httpResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task get_customer_by_id_returns_not_found()
        {
            var httpResponse = await _client.GetAsync(Get.CustomerById(int.MaxValue));

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode); 
        }

        [Fact]
        public async Task create_customer_returns_created()
        {
            var customer = GetTestCustomer();
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(Post.CreateCustomer, content);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);

            // new id is returned in header
            var id = GetIdFromLocationHeader(httpResponse);
            Assert.True(id > 0);
        }

        [Fact]
        public async Task create_customer_returns_bad_request()
        {
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(Post.CreateCustomer, content);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task delete_customer_returns_no_content()
        {
            var customer = GetTestCustomer();
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync(Post.CreateCustomer, content);

            var id = GetIdFromLocationHeader(createResponse);

            var deleteResponse = await _client.DeleteAsync(Delete.CustomerById(id));

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }


        [Fact]
        public async Task delete_customer_returns_bad_request()
        {
            var httpResponse = await _client.DeleteAsync(Delete.CustomerById(0));

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task delete_customer_returns_not_found()
        {
            var httpResponse = await _client.DeleteAsync(Delete.CustomerById(int.MaxValue));

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task update_customer_returns_created()
        {
            var customer = GetTestCustomer();
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync(Post.CreateCustomer, content);

            var id = GetIdFromLocationHeader(createResponse);

            // change customer details
            var newLastName = "Persons";
            customer.LastName = newLastName;
            content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var updateResponse = await _client.PutAsync(Put.CustomerById(id), content);

            Assert.Equal(HttpStatusCode.Created, updateResponse.StatusCode);
        }

        [Fact]
        public async Task update_customer_returns_bad_request()
        {
            var customer = GetTestCustomer();
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(Put.CustomerById(0), content);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task update_customer_returns_not_found()
        {
            var customer = GetTestCustomer();
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(Put.CustomerById(int.MaxValue), content);

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        private CustomerCommandDTO GetTestCustomer()
        {
            return new CustomerCommandDTO { FirstName = "Test", LastName = "Customer", DateOfBirth = new DateTime(2000, 12, 15) };
        }

        private int GetIdFromLocationHeader(HttpResponseMessage response)
        {
            var id = int.Parse(response.Headers.Location.Segments.Last());
            return id;
        }
    }
}
