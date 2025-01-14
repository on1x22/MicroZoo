using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.Models.Users
{
    public class UserWithRoles : User
    {
        public List<Role>? Roles { get; set; }
    }
}
