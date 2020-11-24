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
        private readonly IMedicineScheduleService _medicineScheduleService;
        public MedicineService(IMedicineRepo medicineRepository, IMedicineScheduleService medicineScheduleService)
        {
            _medicineRepository = medicineRepository;
            _medicineScheduleService = medicineScheduleService;
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
            Medicine medicineOld = await _medicineRepository.GetByIdToEditAsync(medicine.Id);
            if (medicineOld == null)
            {
                throw new ApiException("Medicine not found!");
            }

            if ((await _medicineScheduleService.GetAllByMedicineIdAsync(medicine.Id)).Any())
            {
                throw new ApiException("Medicine cannot be edited!");
            }

            IEnumerable<Medicine> existingMedicines = (await _medicineRepository.GetAllByNameAsync(medicine.Name))
                .Where(med => med.Id != medicine.Id).Where(med => IsMedicineEqual(medicine, med));

            if (existingMedicines.Any())
            {
                throw new ApiException("Medicine with such parameters already exists!", 400);
            }                    
            
            return await _medicineRepository.EditAsync(medicine, medicineOld);
        }

        public async Task<IEnumerable<Medicine>> GetAllAsync()
        {
            return await _medicineRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Medicine>> GetAllByNameAsync(string name)
        {
            return await _medicineRepository.GetAllByNameAsync(name);
        }

        public async Task<Medicine> GetByIdAsync(Guid id)
        {
            return await _medicineRepository.GetByIdAsync(id);
        }

        public async Task<Medicine> GetByNameAsync(string name)
        {
            return await _medicineRepository.GetByNameAsync(name);
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
