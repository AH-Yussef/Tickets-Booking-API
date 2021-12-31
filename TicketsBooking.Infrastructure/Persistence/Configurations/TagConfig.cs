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
    public class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(tag => tag.keyword);

            builder.Property(tag => tag.keyword)
                .HasMaxLength(50)
                .IsRequired();

            // many to many in EventTag
            // has to be checked again
            //builder.HasMany(tag => tag.events)
               // .WithMany(eventRelation => eventRelation.Tags);
        }
    }
}
