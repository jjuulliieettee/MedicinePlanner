using System;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.Data.Models
{
    public class FoodSchedule
    {
        public Guid Id { get; set; }        

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime TimeOfFirstMeal { get; set; }

        [Required]
        public int NumberOfMeals { get; set; }

        public Guid MedicineScheduleId { get; set; }

        public virtual MedicineSchedule MedicineSchedule { get; set; }

    }
}
