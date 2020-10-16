using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IFoodScheduleRepo
    {
        FoodSchedule GetFoodScheduleById(string id);
        FoodSchedule GetFoodScheduleByMedicineId(string medicineId);
        FoodSchedule AddFoodSchedule(FoodSchedule foodSchedule);
        FoodSchedule EditFoodSchedule(FoodSchedule foodSchedule);
        FoodSchedule DeleteFoodSchedule(FoodSchedule foodSchedule);
    }
}
