using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TicketsBooking.Infrastructure.Persistence;

namespace TicketsBooking.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseMySQL("Server=localhost;port=3306;database=TicketsBookingDB;user=root;password=gold-berg;");
            return new AppDbContext(builder.Options);
        }
    }
}
