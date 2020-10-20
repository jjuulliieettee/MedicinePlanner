using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class PharmaceuticalFormCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
