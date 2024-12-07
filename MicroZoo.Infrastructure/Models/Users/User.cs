using Microsoft.AspNetCore.Identity;

namespace MicroZoo.Infrastructure.Models.Users
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool Deleted { get; set; }
    }
}
