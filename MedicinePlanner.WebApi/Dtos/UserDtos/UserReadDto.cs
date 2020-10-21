using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class UserReadDto
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
