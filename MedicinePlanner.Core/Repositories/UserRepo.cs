using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationContext _context;
        public UserRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
