using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IMedicineRepo
    {
        Task<IEnumerable<Medicine>> GetAll();
        Task<IEnumerable<Medicine>> GetAllByName(string name);
        Task<Medicine> GetById(Guid id);
        Task<Medicine> GetByName(string name);
        Medicine Add(Medicine medicine);
        Medicine Edit(Medicine medicine);

    }
}
