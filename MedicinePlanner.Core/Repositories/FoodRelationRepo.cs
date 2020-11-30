using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Enums;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<FoodRelation> AddAsync(FoodRelation foodRelation)
        {
            await _context.FoodRelations.AddAsync(foodRelation);
            await _context.SaveChangesAsync();
            return foodRelation;
        }

        public async Task DeleteAsync(FoodRelation foodRelation)
        {
            _context.Remove(foodRelation);
            await _context.SaveChangesAsync();
        }

        public async Task<FoodRelation> EditAsync(FoodRelation foodRelation)
        {
            _context.FoodRelations.Update(foodRelation);
            await _context.SaveChangesAsync();
            return foodRelation;
        }

        public async Task<IEnumerable<FoodRelation>> GetAllAsync()
        {
            return await _context.FoodRelations.ToListAsync();
        }

        public async Task<FoodRelation> GetByIdAsync(FoodRelationType id)
        {
            return await _context.FoodRelations
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(fr => fr.Id == id);
        }

        public async Task<FoodRelation> GetByNameAsync(string name)
        {
            return await _context.FoodRelations
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(fr => fr.Name == name);
        }
    }
}
