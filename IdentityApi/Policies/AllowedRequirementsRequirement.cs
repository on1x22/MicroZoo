using Microsoft.AspNetCore.Authorization;

namespace MicroZoo.IdentityApi.Policies
{
    public class AllowedRequirementsRequirement : IAuthorizationRequirement
    {
        public List<string> AllowedRequirements { get; set; }

        public AllowedRequirementsRequirement(List<string> allowedRequirements)
        {
            AllowedRequirements = allowedRequirements;
        }
    }
}
