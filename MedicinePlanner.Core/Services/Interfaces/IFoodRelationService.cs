using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Services.Interfaces
{
    public interface IFoodRelationService
    {
        Task<IEnumerable<FoodRelation>> GetAll();
        Task<FoodRelation> GetById(Guid id);
        Task<FoodRelation> GetByName(string name);
        Task<FoodRelation> Add(FoodRelation foodRelation);
        Task<FoodRelation> Edit(FoodRelation foodRelation);
        Task Delete(Guid id);
    }
}
