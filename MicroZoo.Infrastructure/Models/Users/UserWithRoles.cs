using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.Models.Users
{
    /// <summary>
    /// Provides main information about user and his roles
    /// </summary>
    public class UserWithRoles : User
    {
        /// <summary>
        /// Roles of user
        /// </summary>
        public List<Role>? Roles { get; set; }
    }
}
