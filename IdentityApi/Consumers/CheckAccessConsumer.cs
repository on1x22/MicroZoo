using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Consumers
{
    public class CheckAccessConsumer : IConsumer<CheckAccessRequest>
    {
        private readonly JwtHandler _jwtHandler;
        private readonly UserManager<User> _userManager;
        private readonly IdentityApiDbContext _dbContext;

        public CheckAccessConsumer(JwtHandler jwtHandler,
                                   UserManager<User> userManager,
                                   IdentityApiDbContext dbContext)
        {
            _jwtHandler = jwtHandler;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<CheckAccessRequest> context)
        {
            var response = new CheckAccessResponse();

            var claimsPrincipal = _jwtHandler.GetPrincipalFromExpiredToken(context.Message.AccessToken!);
            if (claimsPrincipal == null)
            {
                response.IsAuthenticated = false;
                await context.RespondAsync(response);
            }

            if (!claimsPrincipal!.Identity!.IsAuthenticated)
            {
                response.IsAuthenticated = false;
                await context.RespondAsync(response);
            }

            if (context.Message.Policies == null || context.Message.Policies.Count == 0)
            {
                response.IsAccessConfirmed = false;
                await context.RespondAsync(response);
            }

            var userName = claimsPrincipal.Identity.Name;
            var checkedPolicies = context.Message.Policies;

            var user = await _userManager.FindByNameAsync(userName!);
            if (user == null)            
                response.IsAuthenticated = false;
            
            if (user!.Deleted == true)
                response.IsAuthenticated = false;

            var rolesOfUser = _dbContext.Users.Where(usr => usr.Id == user!.Id)
                .Join(_dbContext.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new
                {
                    UserId = ur.UserId,
                    RoleId = ur.RoleId
                })
                .Join(_dbContext.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => new
                {
                    r.Id
                })
                .Select(x => x.Id);

            var allowedRequirementsOfUser = await rolesOfUser
                .Join(_dbContext.RoleRequirements,
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

            var isRequirementsMatch = checkedPolicies!.Any(req =>
                allowedRequirementsOfUser.Contains(req));
            if (!isRequirementsMatch)
                response.IsAccessConfirmed = false;
            else
            {
                response.IsAuthenticated = true;
                response.IsAccessConfirmed = true;
            }

            await context.RespondAsync(response);
        }
    }
}
