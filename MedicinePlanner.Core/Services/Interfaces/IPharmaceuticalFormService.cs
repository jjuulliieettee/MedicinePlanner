using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.Interfaces
{
    public interface IPharmaceuticalFormService
    {
        Task<IEnumerable<PharmaceuticalForm>> GetAll();
        Task<PharmaceuticalForm> GetById(Guid id);
        Task<PharmaceuticalForm> GetByName(string name);
        Task<PharmaceuticalForm> Add(PharmaceuticalForm pharmaceuticalForm);
        Task<PharmaceuticalForm> Edit(PharmaceuticalForm pharmaceuticalForm);
        Task Delete(Guid id);
    }
}
