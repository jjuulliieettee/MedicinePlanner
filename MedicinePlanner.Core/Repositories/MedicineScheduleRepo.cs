using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories
{
    public class MedicineScheduleRepo : IMedicineScheduleRepo
    {
        private readonly ApplicationContext _context;
        public MedicineScheduleRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicineSchedule>> AddAsync(IEnumerable<MedicineSchedule> medicineSchedules)
        {
            await _context.MedicineSchedules.AddRangeAsync(medicineSchedules);
            await _context.SaveChangesAsync();
            return medicineSchedules;
        }

        public async Task DeleteAsync(MedicineSchedule medicineSchedule)
        {
            _context.MedicineSchedules.Remove(medicineSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task<MedicineSchedule> EditAsync(MedicineSchedule medicineSchedule)
        {
            _context.MedicineSchedules.Update(medicineSchedule);
            await _context.SaveChangesAsync();
            return medicineSchedule;
        }

        public async Task<IEnumerable<MedicineSchedule>> GetAllByMedicineNameAndUserIdAsync(string medicineName, Guid userId)
        {
            return await _context.MedicineSchedules
                                 .Include(med => med.Medicine)
                                 .Include(fr => fr.Medicine.FoodRelation)
                                 .Include(pf => pf.Medicine.PharmaceuticalForm)
                                 .Include(user => user.User)
                                 .Include(fs => fs.FoodSchedules)
                                 .Where(ms => ms.UserId == userId)
                                 .Where(ms => medicineName == null || ms.Medicine.Name.Contains(medicineName))
                                 .Where(ms => ms.EndDate.Date >= DateTime.UtcNow.Date)
                                 .OrderBy(ms => ms.StartDate)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<MedicineSchedule>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.MedicineSchedules
                                 .Include(med => med.Medicine)
                                 .Include(fr => fr.Medicine.FoodRelation)
                                 .Include(pf => pf.Medicine.PharmaceuticalForm)
                                 .Include(user => user.User)
                                 .Include(fs => fs.FoodSchedules)
                                 .Where(ms => ms.UserId == userId)
                                 .Where(ms => ms.EndDate.Date >= DateTime.UtcNow.Date)
                                 .OrderBy(ms => ms.StartDate)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<MedicineSchedule>> GetAllByUserIdAndMedicineIdAsync(Guid userId, Guid medicineId)
        {
            return await _context.MedicineSchedules
                                 .Include(med => med.Medicine)
                                 .Include(fr => fr.Medicine.FoodRelation)
                                 .Include(pf => pf.Medicine.PharmaceuticalForm)
                                 .Include(user => user.User)
                                 .Include(fs => fs.FoodSchedules)
                                 .Where(ms => ms.UserId == userId)
                                 .Where(ms => ms.MedicineId == medicineId)
                                 .Where(ms => ms.EndDate.Date >= DateTime.UtcNow.Date)
                                 .OrderBy(ms => ms.StartDate)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<MedicineSchedule>> GetAllByMedicineIdAsync(Guid medicineId)
        {
            return await _context.MedicineSchedules
                                 .Where(ms => ms.MedicineId == medicineId)
                                 .Where(ms => ms.EndDate.Date >= DateTime.UtcNow.Date)
                                 .ToListAsync();
        }

        public async Task<MedicineSchedule> GetByIdAsync(Guid id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _context.MedicineSchedules
                                 .AsNoTracking()
                                 .Include(ms => ms.Medicine)
                                 .ThenInclude(med => med.FoodRelation)
                                 .Include(ms => ms.Medicine.PharmaceuticalForm)
                                 .Include(ms => ms.FoodSchedules)
                                 .FirstOrDefaultAsync(ms => ms.Id == id);
        }
    }
}
