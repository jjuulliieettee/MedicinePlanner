using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IFoodRelationRepo
    {
        Task<IEnumerable<FoodRelation>> GetAll();
        Task<FoodRelation> GetFoodRelationById(Guid id);
        Task<FoodRelation> GetFoodRelationByName(string name);
        FoodRelation AddFoodRelation(FoodRelation foodRelation);
        FoodRelation EditFoodRelation(FoodRelation foodRelation);
    }
}
