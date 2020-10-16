using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IPharmaceuticalForm
    {
        Task<IEnumerable<PharmaceuticalForm>> GetAll();
        Task<PharmaceuticalForm> GetPharmaceuticalFormById(Guid id);
        Task<PharmaceuticalForm> GetPharmaceuticalFormByName(string name);
        PharmaceuticalForm AddPharmaceuticalForm(PharmaceuticalForm pharmaceuticalForm);
        PharmaceuticalForm EditPharmaceuticalForm(PharmaceuticalForm pharmaceuticalForm);
    }
}
