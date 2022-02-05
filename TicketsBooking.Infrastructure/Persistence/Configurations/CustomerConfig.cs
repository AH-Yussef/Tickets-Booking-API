using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(customer => customer.Email);

            builder.Property(customer => customer.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(customer => customer.Password)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(customer => customer.ValidationToken)
                   .HasMaxLength(30);
        }
    }
}
