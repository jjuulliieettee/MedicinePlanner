using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class UserReadDto
    {
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Photo { get; set; }
    }
}
