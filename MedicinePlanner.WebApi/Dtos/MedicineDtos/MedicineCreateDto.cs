using MedicinePlanner.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class MedicineCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ActiveSubstance { get; set; }

        [Required]
        public int Dosage { get; set; }

        [Required]
        public int NumberOfTakes { get; set; }

        public int? FoodInterval { get; set; }

        public PharmaceuticalFormType PharmaceuticalFormId { get; set; }

        public FoodRelationType FoodRelationId { get; set; }
    }
}
