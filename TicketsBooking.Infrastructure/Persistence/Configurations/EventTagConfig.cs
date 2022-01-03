using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Infrastructure.Persistence.Configurations
{
    public class EventTagConfig : IEntityTypeConfiguration<EventTag>
    {
        public void Configure(EntityTypeBuilder<EventTag> builder)
        {
            builder.HasKey(et => new { et.EventId, et.keyword });

            builder.HasOne(e => e.eventRelation)
                .WithMany(et => et.Tags)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.tag)
                .WithMany(et => et.events)
                .HasForeignKey(t => t.keyword)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
