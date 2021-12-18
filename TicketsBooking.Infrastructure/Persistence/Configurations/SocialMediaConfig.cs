using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class SocialMediaConfig : IEntityTypeConfiguration<SocialMedia>
    {
        public void Configure(EntityTypeBuilder<SocialMedia> builder)
        {
            builder.HasKey(eventProvider => eventProvider.Link);

            builder.Property(eventProvider => eventProvider.Link)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(eventProvider => eventProvider.Type)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
