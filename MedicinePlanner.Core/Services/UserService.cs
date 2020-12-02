using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using System;
using System.Threading.Tasks;
using MedicinePlanner.Core.Resources;

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
                throw new ApiException(MessagesResource.USER_ALREADY_EXISTS, 400);
            }
            return await _userRepo.AddAsync(user);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _userRepo.GetByIdAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepo.GetByEmailAsync(email);
        }

        public async Task<User> UpdateAsync(User user)
        {
            return await _userRepo.UpdateAsync(user);
        }
    }
}
