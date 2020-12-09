using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IFoodScheduleRepo
    {
        Task<FoodSchedule> GetByIdAsync(Guid id);
        Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdAsync(Guid medicineScheduleId);
        Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdRangeAsync(MedicineSchedule[] medicineSchedules);
        Task<IEnumerable<FoodSchedule>> GetAllByDateAndUserIdAsync(DateTime date, Guid userId);
        Task<IEnumerable<FoodSchedule>> GetAllByDateRangeAndUserIdAsync(DateTime[] dates, Guid userId);
        Task<FoodSchedule> GetByDateAsync(DateTime date, Guid medicineScheduleId);
        Task<IEnumerable<FoodSchedule>> AddAsync(IEnumerable<FoodSchedule> foodSchedules);
        Task<FoodSchedule> EditAsync(FoodSchedule foodSchedule);
        Task<IEnumerable<FoodSchedule>> EditAllAsync(IEnumerable<FoodSchedule> foodSchedules);
        Task DeleteAllAsync(IEnumerable<FoodSchedule> foodSchedules);
        Task DeleteAsync(FoodSchedule foodSchedule);
    }
}
