using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Enums;
using MedicinePlanner.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicinePlanner.Core.Resources;

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
                throw new ApiException(MessagesResource.PHARMACEUTICAL_FORM_ALREADY_EXISTS, 400);
            }
            return await _pharmaceuticalFormRepo.AddAsync(pharmaceuticalForm);
        }

        public async Task DeleteAsync(PharmaceuticalFormType id)
        {
            PharmaceuticalForm pharmaceuticalForm = await _pharmaceuticalFormRepo.GetByIdAsync(id);
            
            if (pharmaceuticalForm == null)
            {
                throw new ApiException(MessagesResource.PHARMACEUTICAL_FORM_NOT_FOUND);
            }

            if (pharmaceuticalForm.Medicine != null)
            {
                throw new ApiException(MessagesResource.PHARMACEUTICAL_FORM_NOT_DELETABLE, 400);
            }
            await _pharmaceuticalFormRepo.DeleteAsync(pharmaceuticalForm);
        }

        public async Task<PharmaceuticalForm> EditAsync(PharmaceuticalForm pharmaceuticalForm)
        {
            PharmaceuticalForm pharmaceuticalFormOld = await _pharmaceuticalFormRepo.GetByIdAsync(pharmaceuticalForm.Id);
            if (pharmaceuticalFormOld == null)
            {
                throw new ApiException(MessagesResource.PHARMACEUTICAL_FORM_NOT_FOUND);
            }

            if(pharmaceuticalFormOld.Medicine != null)
            {
                throw new ApiException(MessagesResource.PHARMACEUTICAL_FORM_NOT_EDITABLE, 400);
            }

            PharmaceuticalForm pharmaceuticalFormInDb = await _pharmaceuticalFormRepo.GetByNameAsync(pharmaceuticalForm.Name);
            if (pharmaceuticalFormInDb != null && pharmaceuticalFormInDb.Id != pharmaceuticalForm.Id)
            {
                throw new ApiException(MessagesResource.PHARMACEUTICAL_FORM_ALREADY_EXISTS, 400);
            }

            return await _pharmaceuticalFormRepo.EditAsync(pharmaceuticalForm);
        }

        public async Task<IEnumerable<PharmaceuticalForm>> GetAllAsync()
        {
            return await _pharmaceuticalFormRepo.GetAllAsync();
        }

        public async Task<PharmaceuticalForm> GetByIdAsync(PharmaceuticalFormType id)
        {
            return await _pharmaceuticalFormRepo.GetByIdAsync(id);
        }

        public async Task<PharmaceuticalForm> GetByNameAsync(string name)
        {
            return await _pharmaceuticalFormRepo.GetByNameAsync(name);
        }
    }
}
