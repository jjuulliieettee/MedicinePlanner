using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services
{
    public class PharmaceuticalFormService : IPharmaceuticalFormService
    {
        private readonly IPharmaceuticalFormRepo _pharmaceuticalFormRepo;
        public PharmaceuticalFormService(IPharmaceuticalFormRepo pharmaceuticalFormRepo)
        {
            _pharmaceuticalFormRepo = pharmaceuticalFormRepo;
        }

        public async Task<PharmaceuticalForm> AddAsync(PharmaceuticalForm pharmaceuticalForm)
        {
            if (await _pharmaceuticalFormRepo.GetByNameAsync(pharmaceuticalForm.Name) != null)
            {
                throw new ApiException("This pharmaceutical form already exists!", 400);
            }
            return await _pharmaceuticalFormRepo.AddAsync(pharmaceuticalForm);
        }

        public async Task DeleteAsync(Guid id)
        {
            PharmaceuticalForm pharmaceuticalForm = await _pharmaceuticalFormRepo.GetByIdAsync(id);
            
            if (pharmaceuticalForm == null)
            {
                throw new ApiException("Pharmaceutical form not found!");
            }

            if (pharmaceuticalForm.Medicine != null)
            {
                throw new ApiException("This pharmaceutical form cannot be deleted!", 400);
            }
            await _pharmaceuticalFormRepo.DeleteAsync(pharmaceuticalForm);
        }

        public async Task<PharmaceuticalForm> EditAsync(PharmaceuticalForm pharmaceuticalForm)
        {
            PharmaceuticalForm pharmaceuticalFormOld = await _pharmaceuticalFormRepo.GetByIdAsync(pharmaceuticalForm.Id);
            if (pharmaceuticalFormOld == null)
            {
                throw new ApiException("Pharmaceutical form not found!");
            }

            if(pharmaceuticalFormOld.Medicine != null)
            {
                throw new ApiException("This pharmaceutical form cannot be modified!", 400);
            }

            PharmaceuticalForm pharmaceuticalFormInDb = await _pharmaceuticalFormRepo.GetByNameAsync(pharmaceuticalForm.Name);
            if (pharmaceuticalFormInDb != null && pharmaceuticalFormInDb.Id != pharmaceuticalForm.Id)
            {
                throw new ApiException("This pharmaceutical form already exists!", 400);
            }

            return await _pharmaceuticalFormRepo.EditAsync(pharmaceuticalForm);
        }

        public async Task<IEnumerable<PharmaceuticalForm>> GetAllAsync()
        {
            return await _pharmaceuticalFormRepo.GetAllAsync();
        }

        public async Task<PharmaceuticalForm> GetByIdAsync(Guid id)
        {
            return await _pharmaceuticalFormRepo.GetByIdAsync(id);
        }

        public async Task<PharmaceuticalForm> GetByNameAsync(string name)
        {
            return await _pharmaceuticalFormRepo.GetByNameAsync(name);
        }
    }
}
