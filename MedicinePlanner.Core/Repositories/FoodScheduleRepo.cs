using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;

namespace MedicinePlanner.Core.Repositories
{
    public class FoodScheduleRepo : IFoodScheduleRepo
    {
        private readonly ApplicationContext _context;
        public FoodScheduleRepo(ApplicationContext context)
        {
            _context = context;
        }

        public FoodSchedule Add(FoodSchedule foodSchedule)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule Delete(FoodSchedule foodSchedule)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule Edit(FoodSchedule foodSchedule)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FoodSchedule> GetAllByMedicineScheduleId(Guid medicineScheduleId)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public FoodSchedule GetByMedicineScheduleId(Guid medicineScheduleId)
        {
            throw new NotImplementedException();
        }
    }
}
