using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IPharmaceuticalForm
    {
        Task<IEnumerable<PharmaceuticalForm>> GetAll();
        Task<PharmaceuticalForm> GetById(Guid id);
        Task<PharmaceuticalForm> GetByName(string name);
        PharmaceuticalForm Add(PharmaceuticalForm pharmaceuticalForm);
        PharmaceuticalForm Edit(PharmaceuticalForm pharmaceuticalForm);
    }
}
