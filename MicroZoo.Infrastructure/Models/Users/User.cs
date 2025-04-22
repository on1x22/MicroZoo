using Microsoft.AspNetCore.Identity;

namespace MicroZoo.Infrastructure.Models.Users
{
    /// <summary>
    /// Provides main information about user
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// First name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Time when refresh token will expire
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }

        /// <summary>
        /// Indicates that the instance can be deleted
        /// </summary>
        public bool Deleted { get; set; }
    }
}
