using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class FoodScheduleAddDto
    {
        [Required]
        public int NumberOfMeals { get; set; }

        [Required]
        public DateTime TimeOfFirstMeal { get; set; }

        [Required]
        public IEnumerable<MedicineScheduleAddDto> MedicineSchedules { get; set; }
    }
}
