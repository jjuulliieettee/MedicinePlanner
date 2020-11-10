using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepo _medicineRepository;
        public MedicineService(IMedicineRepo medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public async Task<Medicine> AddAsync(Medicine medicine)
        {
            IEnumerable<Medicine> medicines = (await _medicineRepository.GetAllByNameAsync(medicine.Name))
                .Where(med => IsMedicineEqual(medicine, med));
            
            if (medicines.Any())
            {
                throw new ApiException("Medicine with such parameters already exists!", 400);
            }
            return await _medicineRepository.AddAsync(medicine);
        }

        public async Task<Medicine> EditAsync(Medicine medicine)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Medicine>> GetAllAsync()
        {
            return _medicineRepository.GetAllAsync();
        }

        public Task<IEnumerable<Medicine>> GetAllByNameAsync(string name)
        {
            return _medicineRepository.GetAllByNameAsync(name);
        }

        public Task<Medicine> GetByIdAsync(Guid id)
        {
            return _medicineRepository.GetByIdAsync(id);
        }

        public Task<Medicine> GetByNameAsync(string name)
        {
            return _medicineRepository.GetByNameAsync(name);
        }

        private bool IsMedicineEqual(Medicine med1, Medicine med2)
        {
            return med1.Name.ToLower() == med2.Name.ToLower()
                && med1.PharmaceuticalFormId == med2.PharmaceuticalFormId
                && med1.Dosage == med2.Dosage
                && med1.NumberOfTakes == med2.NumberOfTakes;
        }
    }
}
