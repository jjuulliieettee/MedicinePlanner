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
        Task<Medicine> GetMedicineById(Guid id);
        Task<Medicine> GetMedicineByName(string name);
        Medicine AddMedicine(Medicine medicine);
        Medicine EditMedicine(Medicine medicine);

    }
}
