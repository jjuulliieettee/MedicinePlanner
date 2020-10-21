using MedicinePlanner.Data.Models;
using System;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetById(Guid id);
        Task<User> GetByEmail(string email);
        Task<User> Add(User user);
    }
}
