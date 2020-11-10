using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services
{
    public class FoodRelationService : IFoodRelationService
    {
        private readonly IFoodRelationRepo _foodRelationRepo;
        public FoodRelationService(IFoodRelationRepo foodRelationRepo)
        {
            _foodRelationRepo = foodRelationRepo;
        }

        public async Task<FoodRelation> AddAsync(FoodRelation foodRelation)
        {
            if (await _foodRelationRepo.GetByNameAsync(foodRelation.Name) != null)
            {
                throw new ApiException("This food relation already exists!", 400);
            }
            return await _foodRelationRepo.AddAsync(foodRelation);
        }

        public async Task DeleteAsync(Guid id)
        {
            FoodRelation foodRelation = await _foodRelationRepo.GetByIdAsync(id);

            if (foodRelation == null)
            {
                throw new ApiException("Food relation not found!");
            }

            if (foodRelation.Medicine != null)
            {
                throw new ApiException("This food relation cannot be deleted!", 400);
            }
            await _foodRelationRepo.DeleteAsync(foodRelation);
        }

        public async Task<FoodRelation> EditAsync(FoodRelation foodRelation)
        {
            FoodRelation foodRelationOld = await _foodRelationRepo.GetByIdAsync(foodRelation.Id);
            if (foodRelationOld == null)
            {
                throw new ApiException("Food relation not found!");
            }

            if (foodRelationOld.Medicine != null)
            {
                throw new ApiException("This food relation cannot be modified!", 400);
            }

            FoodRelation foodRelationInDb = await _foodRelationRepo.GetByNameAsync(foodRelation.Name);
            if (foodRelationInDb != null && foodRelationInDb.Id != foodRelation.Id)
            {
                throw new ApiException("This food relation already exists!", 400);
            }

            return await _foodRelationRepo.EditAsync(foodRelation);
        }

        public Task<IEnumerable<FoodRelation>> GetAllAsync()
        {
            return _foodRelationRepo.GetAllAsync();
        }

        public Task<FoodRelation> GetByIdAsync(Guid id)
        {
            return _foodRelationRepo.GetByIdAsync(id);
        }

        public Task<FoodRelation> GetByNameAsync(string name)
        {
            return _foodRelationRepo.GetByNameAsync(name);
        }
    }
}
