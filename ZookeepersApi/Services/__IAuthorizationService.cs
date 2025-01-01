using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface __IAuthorizationService
    {
        public Task<CheckAccessResponse> IsResourceAccessConfirmed(string accessToken, List<string> endpointPolicies);
    }
}
