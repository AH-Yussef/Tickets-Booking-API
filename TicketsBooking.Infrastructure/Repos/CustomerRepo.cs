using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Application.Components.Customers.DTOs.Query;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Infrastructure.Persistence;

namespace TicketsBooking.Infrastructure.Repos
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _dbContext;
        private readonly Random _random;

        public CustomerRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _random = new Random();
        }
        public async Task<bool> Approve(string Email)
        {
            var customer = _dbContext.Customers.Find(Email.ToLower());
            customer.Accepted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<Customer>> GetAll(GetAllUsersQuery query)
        {
            var result = _dbContext.Customers.AsQueryable();

            string searchTarget = query.searchTarget;
            if (!string.IsNullOrEmpty(searchTarget))
            {
                result = result.Where(record => record.Name.Contains(searchTarget));
            }
            result = result.Skip((query.pageNumber - 1) * query.pageSize).Take(query.pageSize);
            return await result.ToListAsync();
        }
        public async Task<Customer> GetSingleByEmail(string email)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(customer => customer.Email == email.ToLower());
        }

        public async Task<Customer> Register(RegisterCustomerCommand command)
        {
            var newCustomer = new Customer
            {
                Name = command.Name.ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(command.Password),
                Email = command.Email.ToLower(),
                Accepted = false,
                ValidationToken = generateRandomValidationToken(),
            };

            await _dbContext.Customers.AddAsync(newCustomer);
            await _dbContext.SaveChangesAsync();

            return newCustomer;
        }
        // implemeted in phase 3
        public Task<Customer> UpdateInfo()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string Email)
        {
            var entity = _dbContext.Customers.Find(Email);
            _dbContext.Customers.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private string generateRandomValidationToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 20)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
