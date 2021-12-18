using TicketsBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.Id)
                .HasMaxLength(300);

            //Name
            builder.Property(e => e.Title)
                .HasMaxLength(300);

            //Donation
            builder.HasMany(e => e.Participants)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
