using MedicinePlanner.Data.Enums;
using MedicinePlanner.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class MedicineReadDto
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

        public PharmaceuticalFormType PharmaceuticalFormId { get; set; }

        public FoodRelationType FoodRelationId { get; set; }

        public string PharmaceuticalFormName { get; set; }

        public string FoodRelationName { get; set; }
    }
}
