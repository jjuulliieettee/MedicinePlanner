using System;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class MedicineScheduleEditDto
    {
        public Guid Id { get; set; }

        public Guid MedicineId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public DateTimeOffset StartDate { get; set; }

        [Required]
        public DateTimeOffset EndDate { get; set; }
    }
}
