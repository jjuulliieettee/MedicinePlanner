using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }
        
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Photo { get; set; }

        public string Calendar { get; set; }

        public virtual ICollection<MedicineSchedule> MedicineSchedules { get; set; }
    }
}
