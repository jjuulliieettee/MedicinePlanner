using System;

namespace MedicinePlanner.WebApi.Dtos
{
    public class FoodScheduleReadDto
    {
        public Guid Id { get; set; }

        public Guid MedicineScheduleId { get; set; }

        public DateTime Date { get; set; }

        public DateTime TimeOfFirstMeal { get; set; }

        public int NumberOfMeals { get; set; }

    }
}
