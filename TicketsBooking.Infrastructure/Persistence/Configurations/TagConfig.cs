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
            builder.HasKey(tag => tag.Keyword);

            builder.Property(tag => tag.Keyword)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
