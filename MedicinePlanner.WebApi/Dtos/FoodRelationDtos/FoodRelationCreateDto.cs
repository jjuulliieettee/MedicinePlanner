using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class FoodRelationCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
