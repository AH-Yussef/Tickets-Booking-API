using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class PurchaseConfig : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(p => p.PurchaseID);
            builder.Property(p => p.PurchaseID)
                .IsRequired();

            builder.Property(p => p.ReservationDate)
                //.HasDefaultValue(DateTime.Now.Date);
                .IsRequired();

            builder.Property(p => p.TicketsCount)
                .IsRequired();

            builder.Property(p => p.SingleTicketCost)
                .IsRequired();

            /* builder.Navigation(p => p.customerObject)
                 .UsePropertyAccessMode(PropertyAccessMode.Property);

             builder.Navigation(p => p.eventObject)
                 .UsePropertyAccessMode(PropertyAccessMode.Property);
            */
           /*builder.Property(p => p.customerObject)
                 .IsRequired();

            builder.Property(p => p.eventObject)
                .IsRequired();*/
        }
    }
}
