using MedicinePlanner.Data.Enums;
using MedicinePlanner.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicinePlanner.Core.Repositories.Interfaces
{
    public interface IFoodRelationRepo
    {
        Task<IEnumerable<FoodRelation>> GetAllAsync();
        Task<FoodRelation> GetByIdAsync(FoodRelationType id);
        Task<FoodRelation> GetByNameAsync(string name);
        Task<FoodRelation> AddAsync(FoodRelation foodRelation);
        Task<FoodRelation> EditAsync(FoodRelation foodRelation);
        Task DeleteAsync(FoodRelation foodRelation);
    }
}
