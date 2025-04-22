namespace MicroZoo.Infrastructure.Models.Roles
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about a role
    /// </summary>
    public record RoleWithoutIdDto
    {
        /// <summary>
        /// Description of the role
        /// </summary>
        public string? Description {  get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Normalized name
        /// </summary>
        public string? NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// </summary>
        public string? ConcurrencyStamp { get; set; }
    }
}
