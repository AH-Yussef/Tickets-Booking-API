using System.Reflection;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace TicketsBooking.Infrastructure.Persistence
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //DbSets
        public DbSet<Event> Events { get; set; }
        public DbSet<EventProvider> EventProviders { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }

        //Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(EventConfig)));
        }
    }
}
