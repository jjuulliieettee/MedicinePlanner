using System.ComponentModel.DataAnnotations;

namespace MedicinePlanner.WebApi.Dtos
{
    public class GoogleAuthDto
    {
        [Required]
        public string IdToken { get; set; }
    }
}
