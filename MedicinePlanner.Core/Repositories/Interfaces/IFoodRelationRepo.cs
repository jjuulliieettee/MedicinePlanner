using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IFoodRelationRepo
    {
        Task<IEnumerable<FoodRelation>> GetAll();
        Task<FoodRelation> GetById(Guid id);
        Task<FoodRelation> GetByName(string name);
        FoodRelation Add(FoodRelation foodRelation);
        FoodRelation Edit(FoodRelation foodRelation);
    }
}
