using CustomerAPI.Dto;
using CustomerAPI.Model;
using System.Collections.Generic;

namespace CustomerAPI.Mappers
{
    public static class CustomerMapper
    {
        public static Customer MapCustomerCommandDtoToModel(CustomerCommandDTO customerDto)
        {
            return new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                DateOfBirth = customerDto.DateOfBirth
            };
        }

        public static CustomerDTO MapCustomerModelToDto(Customer customer)
        {
            return new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DateOfBirth = customer.DateOfBirth
            };
        }

        public static List<CustomerDTO> MapCustomerModelListToDtoList(List<Customer> customerList)
        {
            var customerDtoList = new List<CustomerDTO>();

            customerList.ForEach(customer => 
                customerDtoList.Add(MapCustomerModelToDto(customer)));

            return customerDtoList;
        }
    }
}
