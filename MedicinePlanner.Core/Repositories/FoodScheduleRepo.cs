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
    public class FoodScheduleRepo : IFoodScheduleRepo
    {
        private readonly ApplicationContext _context;
        public FoodScheduleRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodSchedule>> AddAsync(IEnumerable<FoodSchedule> foodSchedules)
        {
            await _context.FoodSchedules.AddRangeAsync(foodSchedules);
            await _context.SaveChangesAsync();
            return foodSchedules;
        }

        public async Task DeleteAsync(FoodSchedule foodSchedule)
        {
            _context.FoodSchedules.Remove(foodSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync(IEnumerable<FoodSchedule> foodSchedules)
        {
            _context.FoodSchedules.RemoveRange(foodSchedules);
            await _context.SaveChangesAsync();
        }

        public async Task<FoodSchedule> EditAsync(FoodSchedule foodSchedule)
        {
            _context.FoodSchedules.Update(foodSchedule);
            await _context.SaveChangesAsync();
            return foodSchedule;
        }

        public async Task<IEnumerable<FoodSchedule>> EditAllAsync(IEnumerable<FoodSchedule> foodSchedules)
        {
            _context.FoodSchedules.UpdateRange(foodSchedules);
            await _context.SaveChangesAsync();
            return foodSchedules;
        }

        public async Task<IEnumerable<FoodSchedule>> GetAllByDateAndUserIdAsync(DateTimeOffset date, Guid userId)
        {
            return await _context.FoodSchedules
                                 .AsNoTracking()
                                 .Where(fs => fs.MedicineSchedule.UserId == userId)
                                 .Where(fs => fs.Date.Date == date.Date)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdAsync(Guid medicineScheduleId)
        {
            return await _context.FoodSchedules
                                 .AsNoTracking()
                                 .Where(fs => fs.MedicineScheduleId == medicineScheduleId)
                                 .OrderBy(fs => fs.Date)
                                 .ToListAsync();
        }

        public async Task<FoodSchedule> GetByDateAsync(DateTimeOffset date, Guid medicineScheduleId)
        {
            return await _context.FoodSchedules
                                 .AsNoTracking()
                                 .Where(fs => fs.MedicineScheduleId == medicineScheduleId)
                                 .FirstOrDefaultAsync(fs => fs.Date.Date == date.Date);
        }

        public async Task<FoodSchedule> GetByIdAsync(Guid id)
        {
            return await _context.FoodSchedules
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(fs => fs.Id == id);
        }

        public async Task<IEnumerable<FoodSchedule>> GetAllByDateRangeAndUserIdAsync(DateTimeOffset[] dates, Guid userId)
        {
            var datesOnly = dates.Select(x => x.Date);
            return await _context.FoodSchedules
                                 .Where(fs => fs.MedicineSchedule.UserId == userId)
                                 .Where(fs => datesOnly.Contains(fs.Date.Date))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<FoodSchedule>> GetAllByMedicineScheduleIdRangeAsync(MedicineSchedule[] medicineSchedules)
        {
            var idsOnly = medicineSchedules.Select(x => x.Id);
            return await _context.FoodSchedules
                                 .Include(fs => fs.MedicineSchedule)
                                 .ThenInclude(ms => ms.Medicine)
                                 .ThenInclude(med => med.FoodRelation)
                                 .Include(fs => fs.MedicineSchedule.Medicine.PharmaceuticalForm)
                                 .AsNoTracking()
                                 .Where(fs => idsOnly.Contains(fs.MedicineScheduleId))
                                 .Where(fs => fs.Date.Date >= DateTime.UtcNow.Date)
                                 .ToListAsync();
        }
    }
}
