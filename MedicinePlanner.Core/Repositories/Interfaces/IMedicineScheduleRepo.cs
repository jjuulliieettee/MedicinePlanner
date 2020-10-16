using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IMedicineScheduleRepo
    {
        MedicineSchedule GetMedicineScheduleById(string id);
        MedicineSchedule GetMedicineScheduleByUserId(string userId);
        MedicineSchedule AddMedicineSchedule(MedicineSchedule medicineSchedule);
        MedicineSchedule EditMedicineSchedule(MedicineSchedule medicineSchedule);
        MedicineSchedule DeleteMedicineSchedule(MedicineSchedule medicineSchedule);
    }
}
