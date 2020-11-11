using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IMedicineRepo
    {
        Task<IEnumerable<Medicine>> GetAllAsync();
        Task<IEnumerable<Medicine>> GetAllByNameAsync(string name);
        Task<Medicine> GetByIdAsync(Guid id);
        Task<Medicine> GetByIdToEditAsync(Guid id);
        Task<Medicine> GetByNameAsync(string name);
        Task<Medicine> AddAsync(Medicine medicine);
        Task<Medicine> EditAsync(Medicine medicine, Medicine medicineOld);

    }
}
