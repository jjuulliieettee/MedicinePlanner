using System;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.Data.Models
{
    public class PharmaceuticalForm
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
