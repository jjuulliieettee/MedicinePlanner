using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Data;
using MedicinePlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories
{
    class FoodRelationRepo : IFoodRelationRepo
    {
        private readonly ApplicationContext _context;
        public FoodRelationRepo(ApplicationContext context)
        {
            _context = context;
        }

        public FoodRelation AddFoodRelation(FoodRelation foodRelation)
        {
            _context.FoodRelations.AddAsync(foodRelation);
            _context.SaveChangesAsync();
            return foodRelation;
        }

        public FoodRelation EditFoodRelation(FoodRelation foodRelation)
        {
            _context.FoodRelations.Update(foodRelation);
            _context.SaveChangesAsync();
            return foodRelation;
        }

        public async Task<IEnumerable<FoodRelation>> GetAll()
        {
            return await _context.FoodRelations.ToListAsync();
        }

        public async Task<FoodRelation> GetFoodRelationById(Guid id)
        {
            return await _context.FoodRelations.FirstOrDefaultAsync(fr => fr.Id == id);
        }

        public async Task<FoodRelation> GetFoodRelationByName(string name)
        {
            return await _context.FoodRelations.FirstOrDefaultAsync(fr => fr.Name == name);
        }
    }
}
