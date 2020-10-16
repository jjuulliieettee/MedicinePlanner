using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using System;

namespace MedicinePlanner.Core.Repositories
{
    class MedicineScheduleRepo : IMedicineScheduleRepo
    {
        private readonly ApplicationContext _context;
        public MedicineScheduleRepo(ApplicationContext context)
        {
            _context = context;
        }

        public MedicineSchedule AddMedicineSchedule(MedicineSchedule medicineSchedule)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule DeleteMedicineSchedule(MedicineSchedule medicineSchedule)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule EditMedicineSchedule(MedicineSchedule medicineSchedule)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule GetMedicineScheduleById(string id)
        {
            throw new NotImplementedException();
        }

        public MedicineSchedule GetMedicineScheduleByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
