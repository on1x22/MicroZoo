namespace MicroZoo.Infrastructure.Models.Roles
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about an action that can do a role
    /// </summary>
    public record RequirementWithoutIdDto
    {
        /// <summary>
        /// Description of requirement
        /// </summary>
        public string? Name { get; set; }
    }
}
