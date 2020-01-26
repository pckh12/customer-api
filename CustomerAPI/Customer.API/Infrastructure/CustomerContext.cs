using Microsoft.EntityFrameworkCore;
using CustomerAPI.Model;
using CustomerAPI.Infrastructure.EntityConfigurations;

namespace CustomerAPI.Infrastructure
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        }
    }
}
