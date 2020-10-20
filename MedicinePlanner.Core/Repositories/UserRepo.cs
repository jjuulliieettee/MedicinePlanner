using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using System;

namespace MedicinePlanner.Core.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationContext _context;
        public UserRepo(ApplicationContext context)
        {
            _context = context;
        }

        public User Add(User user)
        {
            throw new NotImplementedException();
        }

        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
