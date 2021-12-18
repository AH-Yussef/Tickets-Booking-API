using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class EventProviderConfig : IEntityTypeConfiguration<EventProvider>
    {
        public void Configure(EntityTypeBuilder<EventProvider> builder)
        {
            builder.HasKey(eventProvider => eventProvider.Name);

            builder.Property(eventProvider => eventProvider.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(eventProvider => eventProvider.Verified)
                .HasDefaultValue(false);

            builder.Property(eventProvider => eventProvider.Bio)
                .HasMaxLength(2000);

            builder.HasIndex(eventProvider => eventProvider.WebsiteLink)
                .IsUnique();

            builder.HasMany(eventProvider => eventProvider.SocialMedias)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
