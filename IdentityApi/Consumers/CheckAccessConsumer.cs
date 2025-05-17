using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Consumers
{
    public class CheckAccessConsumer : IConsumer<CheckAccessRequest>
    {
        private readonly JwtHandler _jwtHandler;
        private readonly UserManager<User> _userManager;
        private readonly IdentityApiDbContext _dbContext;
        private readonly ILogger<CheckAccessConsumer> _logger;

        public CheckAccessConsumer(JwtHandler jwtHandler,
                                   UserManager<User> userManager,
                                   IdentityApiDbContext dbContext,
                                   ILogger<CheckAccessConsumer> logger)
        {
            _jwtHandler = jwtHandler;
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CheckAccessRequest> context)
        {
            var response = new CheckAccessResponse();
            response.OperationId = context.Message.OperationId;
            _logger.LogInformation("A request for access was made by user with " +
                "access token {AccessToken}", context.Message.AccessToken);

            var claimsPrincipal = _jwtHandler.GetPrincipalFromToken(context.Message.AccessToken!);
            
            if (claimsPrincipal == null)
            {
                _logger.LogInformation("User with access token {AccessToken} not found", 
                    context.Message.AccessToken!);
                response.IsAuthenticated = false;
                await context.RespondAsync(response);
                return;
            }

            if (!claimsPrincipal!.Identity!.IsAuthenticated)
            {
                _logger.LogInformation("User {Name} is not authenticated", 
                    claimsPrincipal!.Identity!.Name);
                response.IsAuthenticated = false;
                await context.RespondAsync(response);
                return;
            }

            if (context.Message.Policies == null || context.Message.Policies.Count == 0)
            {
                _logger.LogInformation("Access for user {Name} is not confirmed",
                    claimsPrincipal!.Identity!.Name);
                response.IsAccessConfirmed = false;
                await context.RespondAsync(response);
                return;
            }

            var userName = claimsPrincipal.Identity.Name;
            var checkedPolicies = context.Message.Policies;

            var user = await _userManager.FindByNameAsync(userName!);
            if (user == null)
            {
                _logger.LogInformation("User {userName} not found", userName);
                response.IsAuthenticated = false;
            }

            if (user!.Deleted == true)
            {
                _logger.LogInformation("Status of the user {UserName} is \"Deleted\"", 
                    user!.UserName);
                response.IsAuthenticated = false;
            }

            var allowedRequirementsOfUser = await GetAllowedRequirementsOfUser(user);

            var isRequirementsMatch = checkedPolicies!.Any(req =>
                allowedRequirementsOfUser.Contains(req));
            if (!isRequirementsMatch)
            {
                _logger.LogInformation("User {UserName} have not necessary requirements",
                    user!.UserName);
                response.IsAuthenticated = true;
                response.IsAccessConfirmed = false;
            }
            else
            {
                _logger.LogInformation("Access confirm for user {UserName}", user!.UserName);
                response.IsAuthenticated = true;
                response.IsAccessConfirmed = true;
            }

            await context.RespondAsync(response);
        }

        private async Task<List<string>> GetAllowedRequirementsOfUser(User user)
        {
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

            return allowedRequirementsOfUser!;
        }
    }
}
