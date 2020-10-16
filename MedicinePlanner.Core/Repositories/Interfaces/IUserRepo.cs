using MedicinePlanner.Data.Models;
using System;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IUserRepo
    {
        User GetById(Guid id);
        User GetByEmail(string email); 
        User Add(User user);
    }
}
