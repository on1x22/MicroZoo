using System.ComponentModel.DataAnnotations;

namespace MicroZoo.IdentityApi.Models.DTO
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? ClientUri { get; set; }
    }
}
