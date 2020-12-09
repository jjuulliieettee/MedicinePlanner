using System;

namespace MedicinePlanner.WebApi.Dtos
{
    public class MedicineScheduleReadDto
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MedicineReadDto Medicine { get; set; }
    }
}
