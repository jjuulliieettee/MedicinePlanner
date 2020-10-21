using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class UserCreateDto
    {
        [Required]
        public string Email { get; set; }
    }
}
