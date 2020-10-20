using System;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class FoodRelationDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
