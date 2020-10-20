using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories
{
    public class FoodRelationRepo : IFoodRelationRepo
    {
        private readonly ApplicationContext _context;
        public FoodRelationRepo(ApplicationContext context)
        {
            _context = context;
        }

        public FoodRelation Add(FoodRelation foodRelation)
        {
            _context.FoodRelations.AddAsync(foodRelation);
            _context.SaveChangesAsync();
            return foodRelation;
        }

        public async Task Delete(FoodRelation foodRelation)
        {
            _context.Remove(foodRelation);
            await _context.SaveChangesAsync();
        }

        public FoodRelation Edit(FoodRelation foodRelation)
        {
            _context.FoodRelations.Update(foodRelation);
            _context.SaveChangesAsync();
            return foodRelation;
        }

        public async Task<IEnumerable<FoodRelation>> GetAll()
        {
            return await _context.FoodRelations.ToListAsync();
        }

        public async Task<FoodRelation> GetById(Guid id)
        {
            return await _context.FoodRelations.AsNoTracking().FirstOrDefaultAsync(fr => fr.Id == id);
        }

        public async Task<FoodRelation> GetByName(string name)
        {
            return await _context.FoodRelations.AsNoTracking().FirstOrDefaultAsync(fr => fr.Name == name);
        }
    }
}
