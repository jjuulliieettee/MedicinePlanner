using System;

namespace MedicinePlanner.WebApi.Dtos
{
    public class MedicineScheduleReadDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public MedicineReadDto Medicine { get; set; }
    }
}
