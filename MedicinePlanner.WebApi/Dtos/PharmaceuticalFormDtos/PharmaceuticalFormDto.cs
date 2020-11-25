using MedicinePlanner.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class PharmaceuticalFormDto
    {
        public PharmaceuticalFormType Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
