using CustomerAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerAPI.Infrastructure.EntityConfigurations
{
    class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(m => m.FirstName)
                .HasColumnName("FirstName")
                .IsRequired();

            builder.Property(m => m.LastName)
                .HasColumnName("LastName")
                .IsRequired();

            builder.Property(m => m.DateOfBirth)
                .HasColumnName("DateOfBirth")
                .IsRequired();

        }
    }
}
