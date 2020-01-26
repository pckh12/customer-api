using CustomerAPI.Mappers;
using System;
using Xunit;
using System.Collections.Generic;
using CustomerAPI.Dto;

namespace Customer.API.Test.UnitTests
{
    public class CustomerMapperTest
    {
        [Fact]
        public void map_customer_model_to_dto()
        {
            var model = GetTestCustomerModel();

            var dto = CustomerMapper.MapCustomerModelToDto(model);

            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.FirstName, dto.FirstName);
            Assert.Equal(model.LastName, dto.LastName);
            Assert.Equal(model.DateOfBirth, dto.DateOfBirth);
        }

        [Fact]
        public void map_customer_model_list_to_dto_list()
        {
            var model1 = new CustomerAPI.Model.Customer
            {
                Id = 100,
                FirstName = "David",
                LastName = "Robinson",
                DateOfBirth = new DateTime(1978, 5, 1)
            };
            var model2 = new CustomerAPI.Model.Customer
            {
                Id = 100,
                FirstName = "Tim",
                LastName = "Duncan",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            var models = new List<CustomerAPI.Model.Customer>() { model1, model2 };
            

            var dtos = CustomerMapper.MapCustomerModelListToDtoList(models);

            Assert.Equal(model1.Id, dtos[0].Id);
            Assert.Equal(model1.FirstName, dtos[0].FirstName);
            Assert.Equal(model1.LastName, dtos[0].LastName);
            Assert.Equal(model1.DateOfBirth, dtos[0].DateOfBirth);

            Assert.Equal(model2.Id, dtos[1].Id);
            Assert.Equal(model2.FirstName, dtos[1].FirstName);
            Assert.Equal(model2.LastName, dtos[1].LastName);
            Assert.Equal(model2.DateOfBirth, dtos[1].DateOfBirth);
        }

        [Fact]
        public void map_customer_command_dto_to_model()
        {
            var command = GetTestCustomerCommandDto();

            var model = CustomerMapper.MapCustomerCommandDtoToModel(command);

            Assert.Equal(command.FirstName, model.FirstName);
            Assert.Equal(command.LastName, model.LastName);
            Assert.Equal(command.DateOfBirth, model.DateOfBirth);
        }

        private CustomerAPI.Model.Customer GetTestCustomerModel()
        {
            return new CustomerAPI.Model.Customer
            {
                Id = 100,
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = new DateTime(2000, 1, 1)
            };
        }

        private CustomerCommandDTO GetTestCustomerCommandDto()
        {
            return new CustomerCommandDTO
            {
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = new DateTime(2000, 1, 1)
            };
        }


    }
}
