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

        public async Task<User> Add(User user)
        {
            if (await _userRepo.GetByEmail(user.Email) != null)
            {
                throw new ApiException("This user already exists!", 400);
            }
            return _userRepo.Add(user);
        }

        public Task<User> GetById(Guid id)
        {
            return _userRepo.GetById(id);
        }

        public Task<User> GetByEmail(string email)
        {
            return _userRepo.GetByEmail(email);
        }
    }
}
