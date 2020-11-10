using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User> AddAsync(User user)
        {
            if (await _userRepo.GetByEmailAsync(user.Email) != null)
            {
                throw new ApiException("This user already exists!", 400);
            }
            return await _userRepo.AddAsync(user);
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            return _userRepo.GetByIdAsync(id);
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return _userRepo.GetByEmailAsync(email);
        }
    }
}
