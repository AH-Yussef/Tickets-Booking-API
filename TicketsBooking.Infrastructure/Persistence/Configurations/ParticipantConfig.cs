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
    public class ParticipantConfig : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(p => p.ParticipantID);

            builder.Property(p => p.ParticipantID)
                .IsRequired();

            builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

            builder.Property(p => p.Role)
            .HasMaxLength(100)
            .IsRequired();

            builder.Property(p => p.ImageURL);

            builder.Property(p => p.Team)
                .HasMaxLength(50)
                .IsRequired();

            
        }
    }
}
