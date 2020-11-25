using MedicinePlanner.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class FoodRelationDto
    {
        public FoodRelationType Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
