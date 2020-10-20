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

        public Medicine Add(Medicine medicine)
        {
            _context.Medicines.AddAsync(medicine);
            _context.SaveChangesAsync();
            return medicine;
        }

        public Medicine Edit(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
            _context.SaveChangesAsync();
            return medicine;
        }

        public async Task<Medicine> GetById(Guid id)
        {
            return await _context.Medicines.Include(fr => fr.FoodRelation).Include(pf => pf.PharmaceuticalForm)
                .Include(ms => ms.MedicineSchedules).AsNoTracking().FirstOrDefaultAsync(med => med.Id == id);
        }

        public async Task<IEnumerable<Medicine>> GetAll()
        {
            return await _context.Medicines.Include(fr => fr.FoodRelation).Include(pf => pf.PharmaceuticalForm)
                .Include(ms => ms.MedicineSchedules).ToListAsync();
        }

        public async Task<Medicine> GetByName(string name)
        {
            return await _context.Medicines.Include(fr => fr.FoodRelation).Include(pf => pf.PharmaceuticalForm)
                .Include(ms => ms.MedicineSchedules).AsNoTracking().FirstOrDefaultAsync(med => med.Name == name);
        }

        public async Task<IEnumerable<Medicine>> GetAllByName(string name)
        {
            return await _context.Medicines.Include(fr => fr.FoodRelation).Include(pf => pf.PharmaceuticalForm)
                .Include(ms => ms.MedicineSchedules).Where(med => med.Name == name).ToListAsync();
        }
    }
}
