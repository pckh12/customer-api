using CustomerAPI.Infrastructure;
using System;

namespace Customer.API.Test
{
    public static class SeedData
    {
        public static void PopulateTestData(CustomerContext dbContext)
        {
            dbContext.Customers.Add(new CustomerAPI.Model.Customer { FirstName = "Philip", LastName = "Huynh", DateOfBirth = new DateTime(1980, 11, 28) });
            dbContext.Customers.Add(new CustomerAPI.Model.Customer { FirstName = "David", LastName = "Robinson", DateOfBirth = new DateTime(1981, 10, 25) });
            dbContext.Customers.Add(new CustomerAPI.Model.Customer { FirstName = "Tim", LastName = "Duncan", DateOfBirth = new DateTime(1982, 9, 20) });
            dbContext.Customers.Add(new CustomerAPI.Model.Customer { FirstName = "Tony", LastName = "Parker", DateOfBirth = new DateTime(1983, 8, 15) });
            dbContext.Customers.Add(new CustomerAPI.Model.Customer { FirstName = "Timmy", LastName = "Roberts", DateOfBirth = new DateTime(1984, 7, 10) });
            dbContext.SaveChanges();
        }
    }
}
