using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.Data.Models
{
    public class MedicineSchedule
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public Guid UserId { get; set; }

        public Guid MedicineId { get; set; }

        public virtual User User { get; set; }

        public virtual Medicine Medicine { get; set; }

        public virtual ICollection<FoodSchedule> FoodSchedules { get; set; }
    }
}
