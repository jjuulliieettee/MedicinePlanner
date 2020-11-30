using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.Interfaces
{
    public interface IFoodScheduleService
    {
        Task<FoodSchedule> GetByIdAsync(Guid id);
        Task<FoodSchedule> GetByDateAsync(DateTimeOffset date, Guid medicineScheduleId);
        Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdAsync(Guid medicineScheduleId);
        Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdRangeAsync(MedicineSchedule[] medicineSchedules);
        Task AddAsync(FoodSchedule foodSchedule, IEnumerable<MedicineSchedule> medicineSchedules);
        Task<FoodSchedule> EditAsync(FoodSchedule foodSchedule, Guid userId);
        Task EditAllBasedOnFoodScheduleAsync(Guid foodScheduleId, Guid userId);
        Task EditAllBasedOnMedicineScheduleAsync(MedicineSchedule medicineSchedule);
        Task DeleteAsync(Guid id);
        Task DeleteAllAsync(Guid medicineScheduleId);
    }
}
