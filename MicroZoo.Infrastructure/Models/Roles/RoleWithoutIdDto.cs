namespace MicroZoo.Infrastructure.Models.Roles
{
    public record RoleWithoutIdDto
    {
        public string? Description {  get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }
    }
}
