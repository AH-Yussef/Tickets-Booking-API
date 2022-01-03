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
            builder.HasKey(eventRelation => eventRelation.EventID);

            builder.Property(eventRelation => eventRelation.EventID)
                .IsRequired();

            builder.Property(eventRelation => eventRelation.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(eventRelation => eventRelation.Description)
                .HasMaxLength(300);

            builder.Property(eventRelation => eventRelation.Created)
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            builder.Property(eventRelation => eventRelation.AllTickets)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(eventRelation => eventRelation.BoughtTickets)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(eventRelation => eventRelation.ReservationDueDate)
                .IsRequired();

            builder.Property(eventRelation => eventRelation.Location)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(eventRelation => eventRelation.Category)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(eventRelation => eventRelation.SubCategory)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(eventRelation => eventRelation.participants)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(eventRelation => eventRelation.Provider)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            

            // many to many implemented in EventTagConfig file
            // has to be checked again
           // builder.HasMany(eventRelation => eventRelation.Tags)
             //   .WithMany(tag => tag.events);
                


        }
        

    }
}
