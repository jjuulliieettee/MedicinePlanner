using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using System;

namespace MedicinePlanner.Core.Repositories
{
    class UserRepo : IUserRepo
    {
        private readonly ApplicationContext _context;
        public UserRepo(ApplicationContext context)
        {
            _context = context;
        }

        public User AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
