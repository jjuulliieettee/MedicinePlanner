using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using System;

namespace MedicinePlanner.Core.Repositories
{
    class FoodScheduleRepo : IFoodScheduleRepo
    {
        private readonly ApplicationContext _context;
        public FoodScheduleRepo(ApplicationContext context)
        {
            _context = context;
        }

        public FoodSchedule AddFoodSchedule(FoodSchedule foodSchedule)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule DeleteFoodSchedule(FoodSchedule foodSchedule)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule EditFoodSchedule(FoodSchedule foodSchedule)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule GetFoodScheduleById(string id)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule GetFoodScheduleByMedicineId(string medicineId)
        {
            throw new NotImplementedException();
        }
    }
}
