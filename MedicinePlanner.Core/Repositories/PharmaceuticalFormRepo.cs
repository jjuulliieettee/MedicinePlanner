using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories
{
    public class PharmaceuticalFormRepo : IPharmaceuticalFormRepo
    {
        private readonly ApplicationContext _context;
        public PharmaceuticalFormRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PharmaceuticalForm> AddAsync(PharmaceuticalForm pharmaceuticalForm)
        {
            await _context.PharmaceuticalForms.AddAsync(pharmaceuticalForm);
            await _context.SaveChangesAsync();
            return pharmaceuticalForm;
        }

        public async Task DeleteAsync(PharmaceuticalForm pharmaceuticalForm)
        {
            _context.Remove(pharmaceuticalForm);
            await _context.SaveChangesAsync();
        }

        public async Task<PharmaceuticalForm> EditAsync(PharmaceuticalForm pharmaceuticalForm)
        {
            _context.PharmaceuticalForms.Update(pharmaceuticalForm);
            await _context.SaveChangesAsync();
            return pharmaceuticalForm;
        }

        public async Task<IEnumerable<PharmaceuticalForm>> GetAllAsync()
        {
            return await _context.PharmaceuticalForms.ToListAsync();
        }

        public async Task<PharmaceuticalForm> GetByIdAsync(Guid id)
        {
            return await _context.PharmaceuticalForms.AsNoTracking().FirstOrDefaultAsync(pf => pf.Id == id);
        }

        public async Task<PharmaceuticalForm> GetByNameAsync(string name)
        {
            return await _context.PharmaceuticalForms.AsNoTracking().FirstOrDefaultAsync(pf => pf.Name == name);
        }
    }
}
