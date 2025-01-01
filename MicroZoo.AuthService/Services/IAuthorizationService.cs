using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.AuthService.Services
{
    public interface IAuthorizationService
    {
        public Task<CheckAccessResponse> IsResourceAccessConfirmed(Uri identityApiUri, 
            string accessToken, List<string> endpointPolicies);
    }
}
