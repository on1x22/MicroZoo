using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
