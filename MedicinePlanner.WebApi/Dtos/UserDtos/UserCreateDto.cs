using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class UserCreateDto
    {
        [Required]
        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Photo { get; set; }
    }
}
