using System;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class MedicineEditDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ActiveSubstance { get; set; }

        [Required]
        public int Dosage { get; set; }

        [Required]
        public int NumberOfTakes { get; set; }

        public int FoodInterval { get; set; }

        public Guid PharmaceuticalFormId { get; set; }

        public Guid FoodRelationId { get; set; }
    }
}
