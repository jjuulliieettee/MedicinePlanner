using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IFoodScheduleRepo
    {
        FoodSchedule GetById(Guid id);
        IEnumerable<FoodSchedule> GetAllByMedicineScheduleId(Guid medicineScheduleId);
        FoodSchedule GetByDate(DateTime date);
        FoodSchedule GetByMedicineScheduleId(Guid medicineScheduleId);
        FoodSchedule Add(FoodSchedule foodSchedule);
        FoodSchedule Edit(FoodSchedule foodSchedule);
        FoodSchedule Delete(FoodSchedule foodSchedule);
    }
}
