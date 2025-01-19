namespace MicroZoo.Infrastructure.Models.Roles
{
    public class RoleWithRequirements : Role
    {
        public List<Requirement>? Requirements { get; set; }
    }
}
