using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Policies
{
    public class AllowedRequirementsHandler : AuthorizationHandler<AllowedRequirementsRequirement>
    {
        private readonly UserManager<User> _userManager;
        private readonly IdentityApiDbContext _dbContext;

        public AllowedRequirementsHandler(UserManager<User> userManager,
                                           IdentityApiDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       AllowedRequirementsRequirement requirement)
        {
            var calimsPrincipal = context.User;
            if (calimsPrincipal.Identity!.IsAuthenticated == false)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userName = calimsPrincipal.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) 
                context.Fail();

            var rolesOfUser = _dbContext.Users.Join(_dbContext.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new
                {
                    RoleId = ur.RoleId
                }).Join(_dbContext.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => new
                {
                    r.Id
                })
                .Select(x => x.Id);
            
            var allowedRequirementsOfUser = await rolesOfUser.Join(_dbContext.RoleRequirements,
                r => r,
                rr => rr.RoleId,
                (r, rr) => new
                {
                    RequirementId = rr.RequirementId
                }).Join(_dbContext.Requirements,
                rr => rr.RequirementId,
                req => req.Id,
                (rr, req) => new
                {
                    RequirementName = req.Name
                })
                .Select(x => x.RequirementName).ToListAsync();

            var isRequirementsMatch = requirement.AllowedRequirements.All(req =>
                allowedRequirementsOfUser.Contains(req));
            if (!isRequirementsMatch)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }

    }
}
