using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.Interfaces
{
    public interface IMedicineScheduleService
    {
        Task<MedicineSchedule> GetByIdAsync(Guid id);
        Task<IEnumerable<MedicineSchedule>> GetAllByMedicineIdAsync(Guid medicineId);
        Task<IEnumerable<MedicineSchedule>> GetAllByUserIdAsync(Guid userId);
        Task<IEnumerable<MedicineSchedule>> GetAllByMedicineAndUserIdAsync(string medicineName, Guid userId);
        Task<IEnumerable<MedicineSchedule>> AddAsync(IEnumerable<MedicineSchedule> medicineSchedules, Guid userId);
        Task<MedicineSchedule> EditAsync(MedicineSchedule medicineSchedule);
        Task DeleteAsync(Guid id);
    }
}
