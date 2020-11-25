using MedicinePlanner.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.Data.Models
{
    public class Medicine
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
                
        public int? FoodInterval { get; set; }

        public PharmaceuticalFormType PharmaceuticalFormId { get; set; }

        public FoodRelationType FoodRelationId { get; set; }

        public virtual PharmaceuticalForm PharmaceuticalForm { get; set; }

        public virtual FoodRelation FoodRelation { get; set; }

        public virtual ICollection<MedicineSchedule> MedicineSchedules { get; set; }
    }
}
