using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories
{
    class PharmaceuticalFormRepo : IPharmaceuticalForm
    {
        private readonly ApplicationContext _context;
        public PharmaceuticalFormRepo(ApplicationContext context)
        {
            _context = context;
        }

        public PharmaceuticalForm AddPharmaceuticalForm(PharmaceuticalForm pharmaceuticalForm)
        {
            _context.PharmaceuticalForms.AddAsync(pharmaceuticalForm);
            _context.SaveChangesAsync();
            return pharmaceuticalForm;
        }

        public PharmaceuticalForm EditPharmaceuticalForm(PharmaceuticalForm pharmaceuticalForm)
        {
            _context.PharmaceuticalForms.Update(pharmaceuticalForm);
            _context.SaveChangesAsync();
            return pharmaceuticalForm;
        }

        public async Task<IEnumerable<PharmaceuticalForm>> GetAll()
        {
            return await _context.PharmaceuticalForms.ToListAsync();
        }

        public async Task<PharmaceuticalForm> GetPharmaceuticalFormById(Guid id)
        {
            return await _context.PharmaceuticalForms.FirstOrDefaultAsync(pf => pf.Id == id);
        }

        public async Task<PharmaceuticalForm> GetPharmaceuticalFormByName(string name)
        {
            return await _context.PharmaceuticalForms.FirstOrDefaultAsync(pf => pf.Name == name);
        }
    }
}
