using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class AdminConfig : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(admin => admin.Email);

            builder.Property(admin => admin.Name)
                .HasMaxLength(100);

            builder.Property(admin => admin.Password)
                   .HasMaxLength(100)
                   .IsRequired();
        }
    }
}
