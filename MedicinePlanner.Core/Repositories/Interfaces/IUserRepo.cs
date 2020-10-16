using MedicinePlanner.Data.Models;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IUserRepo
    {
        User GetUserById(string id);
        User GetUserByEmail(string email); 
        User AddUser(User user);
    }
}
