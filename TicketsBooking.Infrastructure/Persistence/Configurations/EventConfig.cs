using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(eventEntity => eventEntity.EventID);
            builder.Property(eventEntity => eventEntity.EventID)
                            .HasMaxLength(200);

            builder.Property(eventEntity => eventEntity.EventID)
                .IsRequired();

            builder.Property(eventEntity => eventEntity.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(eventEntity => eventEntity.Description)
                .HasMaxLength(300);

            builder.Property(eventEntity => eventEntity.dateTime)
                .IsRequired();

            builder.Property(eventEntity => eventEntity.AllTickets)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(eventEntity => eventEntity.BoughtTickets)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(eventEntity => eventEntity.ReservationDueDate)
                .IsRequired();

            builder.Property(eventEntity => eventEntity.Location)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(eventRelation => eventRelation.Participants)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(eventRelation => eventRelation.Provider)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(eventRelation => eventRelation.Purchases)
                .WithOne()
                //.HasForeignKey(p => p.eventObject)
                .OnDelete(DeleteBehavior.Cascade);
        }
        

    }
}
