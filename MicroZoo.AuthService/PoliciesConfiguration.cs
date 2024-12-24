using Microsoft.AspNetCore.Authorization;
using MicroZoo.IdentityApi.DbContexts;

namespace MicroZoo.IdentityApi.Policies
{
    public class PoliciesConfiguration
    {
        private readonly IdentityApiDbContext _dbContext;

        public PoliciesConfiguration(IdentityApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static AuthorizationOptions AddAuthorizationOptions(AuthorizationOptions options)
        {
            foreach (var allowedRequirement in AllowedRequirements)
            {
                options.AddPolicy(allowedRequirement, policy =>
                        policy.Requirements.Add(new AllowedRequirementsRequirement(
                            [allowedRequirement])));
            }
            
            return options;
        }

        private static List<string> AllowedRequirements = new List<string>()
        {
            "IdentityApi.Create",
            "IdentityApi.Read",
            "IdentityApi.Update",
            "IdentityApi.Delete"
        };
    }
}
