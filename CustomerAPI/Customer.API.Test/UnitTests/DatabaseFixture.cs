using CustomerAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customer.API.Test.UnitTests
{
    public class DatabaseFixture : IDisposable
    {
        public DbContextOptions<CustomerContext> DbOptions { get; private set; }

        public DatabaseFixture()
        {
            DbOptions = new DbContextOptionsBuilder<CustomerContext>()
                .UseInMemoryDatabase("CustomerMockDb")
                .Options;

            using (var dbContext = new CustomerContext(DbOptions))
            {
                SeedData.PopulateTestData(dbContext);
                dbContext.SaveChanges();
            }
        }

        public void Dispose()
        {

        }
    }
}
