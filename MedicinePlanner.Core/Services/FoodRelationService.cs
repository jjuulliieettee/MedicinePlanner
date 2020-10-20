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

        public async Task<FoodRelation> Add(FoodRelation foodRelation)
        {
            if (await _foodRelationRepo.GetByName(foodRelation.Name) != null)
            {
                throw new ApiException("This food relation already exists!", 400);
            }
            return _foodRelationRepo.Add(foodRelation);
        }

        public async Task Delete(Guid id)
        {
            FoodRelation foodRelation = await _foodRelationRepo.GetById(id);

            if (foodRelation == null)
            {
                throw new ApiException("Food relation not found!");
            }

            if (foodRelation.Medicine != null)
            {
                throw new ApiException("This food relation cannot be deleted!", 400);
            }
            await _foodRelationRepo.Delete(foodRelation);
        }

        public async Task<FoodRelation> Edit(FoodRelation foodRelation)
        {
            FoodRelation foodRelationOld = await _foodRelationRepo.GetById(foodRelation.Id);
            if (foodRelationOld == null)
            {
                throw new ApiException("Food relation not found!");
            }

            if (foodRelationOld.Medicine != null)
            {
                throw new ApiException("This food relation cannot be modified!", 400);
            }

            FoodRelation foodRelationInDb = await _foodRelationRepo.GetByName(foodRelation.Name);
            if (foodRelationInDb != null && foodRelationInDb.Id != foodRelation.Id)
            {
                throw new ApiException("This food relation already exists!", 400);
            }

            return _foodRelationRepo.Edit(foodRelation);
        }

        public Task<IEnumerable<FoodRelation>> GetAll()
        {
            return _foodRelationRepo.GetAll();
        }

        public Task<FoodRelation> GetById(Guid id)
        {
            return _foodRelationRepo.GetById(id);
        }

        public Task<FoodRelation> GetByName(string name)
        {
            return _foodRelationRepo.GetByName(name);
        }
    }
}
