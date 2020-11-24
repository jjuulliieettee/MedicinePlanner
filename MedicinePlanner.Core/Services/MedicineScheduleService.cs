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
    public class MedicineScheduleService : IMedicineScheduleService
    {
        private readonly IMedicineScheduleRepo _medicineScheduleRepo;
        private readonly IFoodScheduleService _foodScheduleService;
        public MedicineScheduleService(IMedicineScheduleRepo medicineScheduleRepo, IFoodScheduleService foodScheduleService)
        {
            _medicineScheduleRepo = medicineScheduleRepo;
            _foodScheduleService = foodScheduleService;
        }

        public async Task<IEnumerable<MedicineSchedule>> AddAsync(IEnumerable<MedicineSchedule> medicineSchedules, Guid userId)
        {
            IEnumerable<MedicineSchedule> existingMedicineSchedules = (await _medicineScheduleRepo.GetAllByUserIdAsync(userId))
                .Where(ms => ms.EndDate.Date >= DateTime.UtcNow.Date);

            foreach (MedicineSchedule medSchedNew in medicineSchedules)
            {
                medSchedNew.UserId = userId;

                foreach (MedicineSchedule medSchedOld in existingMedicineSchedules)
                {
                    if (DoesMedicineScheduleOverlap(medSchedNew, medSchedOld))
                    {
                        throw new ApiException($"Medicine schedules for {medSchedOld.Medicine.Name} overlap!", 400);
                    }
                }
            }
            return await _medicineScheduleRepo.AddAsync(medicineSchedules);
        }

        public async Task DeleteAsync(Guid id)
        {
            MedicineSchedule medicineSchedule = await _medicineScheduleRepo.GetByIdAsync(id);

            if (medicineSchedule == null)
            {
                throw new ApiException("Medicine schedule not found!");
            }

            await _medicineScheduleRepo.DeleteAsync(medicineSchedule);
        }

        public async Task<MedicineSchedule> EditAsync(MedicineSchedule medicineSchedule)
        {
            MedicineSchedule medicineScheduleOld = await _medicineScheduleRepo.GetByIdAsync(medicineSchedule.Id);
            if (medicineScheduleOld == null)
            {
                throw new ApiException("Medicine schedule not found!");
            }

            IEnumerable<MedicineSchedule> existingMedicineSchedules = (await _medicineScheduleRepo.GetAllByUserIdAndMedicineIdAsync
                (medicineSchedule.UserId, medicineSchedule.MedicineId))
                .Where(ms => ms.EndDate.Date >= DateTime.UtcNow.Date).Where(ms => ms.Id != medicineSchedule.Id);

            if (existingMedicineSchedules.Any(ms => DoesMedicineScheduleOverlap(medicineSchedule, ms)))
            {
                throw new ApiException($"Medicine schedules for {medicineScheduleOld.Medicine.Name} overlap!", 400);
            }

            MedicineSchedule medicineScheduleNew = await _medicineScheduleRepo.EditAsync(medicineSchedule);
            await _foodScheduleService.EditAllBasedOnMedicineScheduleAsync(medicineScheduleNew);

            return medicineScheduleNew;
        }

        public async Task<IEnumerable<MedicineSchedule>> GetAllByMedicineAndUserIdAsync(string medicineName, Guid userId)
        {
            return (await _medicineScheduleRepo.GetAllByMedicineNameAndUserIdAsync(medicineName, userId))
                .Where(ms => ms.EndDate.Date >= DateTime.UtcNow.Date);
        }

        public async Task<IEnumerable<MedicineSchedule>> GetAllByMedicineIdAsync(Guid medicineId)
        {
            return await _medicineScheduleRepo.GetAllByMedicineIdAsync(medicineId);
        }

        public async Task<IEnumerable<MedicineSchedule>> GetAllByUserIdAsync(Guid userId)
        {
            return await _medicineScheduleRepo.GetAllByUserIdAsync(userId);
        }

        public async Task<MedicineSchedule> GetByIdAsync(Guid id)
        {
            return await _medicineScheduleRepo.GetByIdAsync(id);
        }

        private bool DoesMedicineScheduleOverlap(MedicineSchedule ms1, MedicineSchedule ms2)
        {
            return ms1.MedicineId == ms2.MedicineId
                && ms1.StartDate <= ms2.EndDate
                && ms1.EndDate >= ms2.StartDate;
        }
    }
}
