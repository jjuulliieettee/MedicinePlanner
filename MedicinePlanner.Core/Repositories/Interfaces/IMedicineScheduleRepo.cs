using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IMedicineScheduleRepo
    {
        Task<MedicineSchedule> GetByIdAsync(Guid id);
        Task<IEnumerable<MedicineSchedule>> GetAllByUserIdAsync(Guid userId);
        Task<IEnumerable<MedicineSchedule>> GetAllByUserIdAndMedicineIdAsync(Guid userId, Guid medicineId);
        Task<IEnumerable<MedicineSchedule>> GetAllByMedicineNameAndUserIdAsync(string medicineName, Guid userId);
        Task<IEnumerable<MedicineSchedule>> AddAsync(IEnumerable<MedicineSchedule> medicineSchedules);
        Task<MedicineSchedule> EditAsync(MedicineSchedule medicineSchedule);
        Task DeleteAsync(MedicineSchedule medicineSchedule);
    }
}
