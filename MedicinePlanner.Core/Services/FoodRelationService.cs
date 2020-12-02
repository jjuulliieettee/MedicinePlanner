using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Repositories.Interfaces;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Enums;
using MedicinePlanner.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicinePlanner.Core.Resources;

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
                throw new ApiException(MessagesResource.FOOD_RELATION_ALREADY_EXISTS, 400);
            }
            return await _foodRelationRepo.AddAsync(foodRelation);
        }

        public async Task DeleteAsync(FoodRelationType id)
        {
            FoodRelation foodRelation = await _foodRelationRepo.GetByIdAsync(id);

            if (foodRelation == null)
            {
                throw new ApiException(MessagesResource.FOOD_RELATION_NOT_FOUND);
            }

            if (foodRelation.Medicine != null)
            {
                throw new ApiException(MessagesResource.FOOD_RELATION_NOT_DELETABLE, 400);
            }
            await _foodRelationRepo.DeleteAsync(foodRelation);
        }

        public async Task<FoodRelation> EditAsync(FoodRelation foodRelation)
        {
            FoodRelation foodRelationOld = await _foodRelationRepo.GetByIdAsync(foodRelation.Id);
            if (foodRelationOld == null)
            {
                throw new ApiException(MessagesResource.FOOD_RELATION_NOT_FOUND);
            }

            if (foodRelationOld.Medicine != null)
            {
                throw new ApiException(MessagesResource.FOOD_RELATION_NOT_EDITABLE, 400);
            }

            FoodRelation foodRelationInDb = await _foodRelationRepo.GetByNameAsync(foodRelation.Name);
            if (foodRelationInDb != null && foodRelationInDb.Id != foodRelation.Id)
            {
                throw new ApiException(MessagesResource.FOOD_RELATION_ALREADY_EXISTS, 400);
            }

            return await _foodRelationRepo.EditAsync(foodRelation);
        }

        public async Task<IEnumerable<FoodRelation>> GetAllAsync()
        {
            return await _foodRelationRepo.GetAllAsync();
        }

        public async Task<FoodRelation> GetByIdAsync(FoodRelationType id)
        {
            return await _foodRelationRepo.GetByIdAsync(id);
        }

        public async Task<FoodRelation> GetByNameAsync(string name)
        {
            return await _foodRelationRepo.GetByNameAsync(name);
        }
    }
}
