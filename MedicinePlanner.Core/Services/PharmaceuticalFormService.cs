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

        public async Task<PharmaceuticalForm> Add(PharmaceuticalForm pharmaceuticalForm)
        {
            if (await _pharmaceuticalFormRepo.GetByName(pharmaceuticalForm.Name) != null)
            {
                throw new ApiException("This pharmaceutical form already exists!", 400);
            }
            return _pharmaceuticalFormRepo.Add(pharmaceuticalForm);
        }

        public async Task Delete(Guid id)
        {
            PharmaceuticalForm pharmaceuticalForm = await _pharmaceuticalFormRepo.GetById(id);
            
            if (pharmaceuticalForm == null)
            {
                throw new ApiException("Pharmaceutical form not found!");
            }

            if (pharmaceuticalForm.Medicine != null)
            {
                throw new ApiException("This pharmaceutical form cannot be deleted!", 400);
            }
            await _pharmaceuticalFormRepo.Delete(pharmaceuticalForm);
        }

        public async Task<PharmaceuticalForm> Edit(PharmaceuticalForm pharmaceuticalForm)
        {
            PharmaceuticalForm pharmaceuticalFormOld = await _pharmaceuticalFormRepo.GetById(pharmaceuticalForm.Id);
            if (pharmaceuticalFormOld == null)
            {
                throw new ApiException("Pharmaceutical form not found!");
            }

            if(pharmaceuticalFormOld.Medicine != null)
            {
                throw new ApiException("This pharmaceutical form cannot be modified!", 400);
            }

            PharmaceuticalForm pharmaceuticalFormInDb = await _pharmaceuticalFormRepo.GetByName(pharmaceuticalForm.Name);
            if (pharmaceuticalFormInDb != null && pharmaceuticalFormInDb.Id != pharmaceuticalForm.Id)
            {
                throw new ApiException("This pharmaceutical form already exists!", 400);
            }

            return _pharmaceuticalFormRepo.Edit(pharmaceuticalForm);
        }

        public Task<IEnumerable<PharmaceuticalForm>> GetAll()
        {
            return _pharmaceuticalFormRepo.GetAll();
        }

        public Task<PharmaceuticalForm> GetById(Guid id)
        {
            return _pharmaceuticalFormRepo.GetById(id);
        }

        public Task<PharmaceuticalForm> GetByName(string name)
        {
            return _pharmaceuticalFormRepo.GetByName(name);
        }
    }
}
