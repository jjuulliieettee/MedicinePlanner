using System;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class FoodScheduleEditDto
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public DateTime TimeOfFirstMeal { get; set; }

        [Required]
        public int NumberOfMeals { get; set; }

        public Guid MedicineScheduleId { get; set; }

    }
}
