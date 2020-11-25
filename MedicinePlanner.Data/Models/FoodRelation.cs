using MedicinePlanner.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.Data.Models
{
    public class FoodRelation
    {
        public FoodRelationType Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Medicine> Medicine { get; set; }
    }
}
