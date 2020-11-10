using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IPharmaceuticalFormRepo
    {
        Task<IEnumerable<PharmaceuticalForm>> GetAllAsync();
        Task<PharmaceuticalForm> GetByIdAsync(Guid id);
        Task<PharmaceuticalForm> GetByNameAsync(string name);
        Task<PharmaceuticalForm> AddAsync(PharmaceuticalForm pharmaceuticalForm);
        Task<PharmaceuticalForm> EditAsync(PharmaceuticalForm pharmaceuticalForm);
        Task DeleteAsync(PharmaceuticalForm pharmaceuticalForm);
    }
}
