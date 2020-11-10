using System;

namespace MedicinePlanner.WebApi.Dtos
{
    public class FoodScheduleReadDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public DateTimeOffset TimeOfFirstMeal { get; set; }

        public int NumberOfMeals { get; set; }

    }
}
