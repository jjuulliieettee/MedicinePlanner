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
    public class MedicineRepo : IMedicineRepo
    {
        private readonly ApplicationContext _context;
        public MedicineRepo (ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Medicine> AddAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();
            return medicine;
        }

        public async Task<Medicine> EditAsync(Medicine medicine, Medicine medicineOld)
        {
            _context.Entry(medicineOld).CurrentValues.SetValues(medicine);
            await _context.SaveChangesAsync();
            return medicine;
        }

        public async Task<Medicine> GetByIdAsync(Guid id)
        {
            return await _context.Medicines
                                 .AsNoTracking()
                                 .Include(med => med.FoodRelation)
                                 .Include(med => med.PharmaceuticalForm)
                                 .Include(med => med.MedicineSchedules)
                                 .FirstOrDefaultAsync(med => med.Id == id);
        }

        public async Task<IEnumerable<Medicine>> GetAllAsync()
        {
            return await _context.Medicines
                                 .Include(med => med.FoodRelation)
                                 .Include(med => med.PharmaceuticalForm)
                                 .Include(med => med.MedicineSchedules)
                                 .OrderBy(med => med.Name)
                                 .ToListAsync();
        }

        public async Task<Medicine> GetByNameAsync(string name)
        {
            return await _context.Medicines
                                 .AsNoTracking()
                                 .Include(med => med.FoodRelation)
                                 .Include(med => med.PharmaceuticalForm)
                                 .Include(med => med.MedicineSchedules)
                                 .FirstOrDefaultAsync(med => med.Name == name);
        }

        public async Task<IEnumerable<Medicine>> GetAllByNameAsync(string name)
        {
            return await _context.Medicines
                                 .Include(med => med.FoodRelation)
                                 .Include(med => med.PharmaceuticalForm)
                                 .Include(med => med.MedicineSchedules)
                                 .Where(med => name == null || med.Name.Contains(name))
                                 .OrderBy(med => med.Name)
                                 .ToListAsync();
        }

        public async Task<Medicine> GetByIdToEditAsync(Guid id)
        {
            return await _context.Medicines
                                 .FirstOrDefaultAsync(med => med.Id == id);
        }
    }
}
