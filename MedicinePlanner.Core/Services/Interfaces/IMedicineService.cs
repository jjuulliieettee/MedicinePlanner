using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<Medicine>> GetAllAsync();
        Task<IEnumerable<Medicine>> GetAllByNameAsync(string name);
        Task<Medicine> GetByIdAsync(Guid id);
        Task<Medicine> GetByNameAsync(string name);
        Task<Medicine> AddAsync(Medicine medicine);
        Task<Medicine> EditAsync(Medicine medicine);
    }
}
