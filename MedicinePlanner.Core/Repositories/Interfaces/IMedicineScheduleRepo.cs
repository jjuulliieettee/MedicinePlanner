using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IMedicineScheduleRepo
    {
        MedicineSchedule GetById(Guid id);
        MedicineSchedule GetByUserId(Guid userId);
        IEnumerable<MedicineSchedule> GetAllByUserId(Guid userId);
        MedicineSchedule GetByMedicineId(Guid medicineId);
        MedicineSchedule GetByMedicineAndUserId(Guid medicineId, Guid userId);
        MedicineSchedule Add(MedicineSchedule medicineSchedule);
        MedicineSchedule Edit(MedicineSchedule medicineSchedule);
        MedicineSchedule Delete(MedicineSchedule medicineSchedule);
    }
}
