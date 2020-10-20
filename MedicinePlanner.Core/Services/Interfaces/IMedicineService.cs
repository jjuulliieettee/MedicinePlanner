using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<Medicine>> GetAll();
        Task<IEnumerable<Medicine>> GetAllByName(string name);
        Task<Medicine> GetById(Guid id);
        Task<Medicine> GetByName(string name);
        Task<Medicine> Add(Medicine medicine);
        Task<Medicine> Edit(Medicine medicine);
    }
}
