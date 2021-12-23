using System.Threading.Tasks;
using TicketsBooking.Application.Components.Admins;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;

namespace TicketsBooking.Infrastructure.Repos
{
    public class AdminRepo : IAdminRepo
    {
        private readonly AppDbContext _dbContext;

        public AdminRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Admin> GetSingle(string email)
        {
            return await _dbContext.Admins.FindAsync(email);
        }
    }
}
