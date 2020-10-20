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

        public async Task<Medicine> Add(Medicine medicine)
        {
            IEnumerable<Medicine> medicines = (await _medicineRepository.GetAllByName(medicine.Name))
                .Where(med => IsMedicineEqual(medicine, med));
            
            if (medicines.Any())
            {
                throw new ApiException("Medicine with such parameters already exists!", 400);
            }
            return _medicineRepository.Add(medicine);
        }

        public async Task<Medicine> Edit(Medicine medicine)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Medicine>> GetAll()
        {
            return _medicineRepository.GetAll();
        }

        public Task<IEnumerable<Medicine>> GetAllByName(string name)
        {
            return _medicineRepository.GetAllByName(name);
        }

        public Task<Medicine> GetById(Guid id)
        {
            return _medicineRepository.GetById(id);
        }

        public Task<Medicine> GetByName(string name)
        {
            return _medicineRepository.GetByName(name);
        }

        private bool IsMedicineEqual(Medicine med1, Medicine med2)
        {
            return med1.Name == med2.Name
                && med1.PharmaceuticalFormId == med2.PharmaceuticalFormId
                && med1.Dosage == med2.Dosage
                && med1.NumberOfTakes == med2.NumberOfTakes;
        }
    }
}
