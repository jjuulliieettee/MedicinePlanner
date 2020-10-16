using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;

namespace MedicinePlanner.Core.Repositories
{
    class MedicineScheduleRepo : IMedicineScheduleRepo
    {
        private readonly ApplicationContext _context;
        public MedicineScheduleRepo(ApplicationContext context)
        {
            _context = context;
        }

        public MedicineSchedule Add(MedicineSchedule medicineSchedule)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule Delete(MedicineSchedule medicineSchedule)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule Edit(MedicineSchedule medicineSchedule)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MedicineSchedule> GetAllByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule GetByMedicineAndUserId(Guid medicineId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule GetByMedicineId(Guid medicineId)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule GetByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
