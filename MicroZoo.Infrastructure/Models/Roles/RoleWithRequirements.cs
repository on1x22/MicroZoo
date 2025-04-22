namespace MicroZoo.Infrastructure.Models.Roles
{
    /// <summary>
    /// Provides information about the role and her requirements that can have a user
    /// </summary>
    public class RoleWithRequirements : Role
    {
        /// <summary>
        /// Role related requirements
        /// </summary>
        public List<Requirement>? Requirements { get; set; }
    }
}
