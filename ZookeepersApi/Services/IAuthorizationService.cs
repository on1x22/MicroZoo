using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IAuthorizationService
    {
        public Task<CheckAccessResponse> IsResourceAccessConfirmed(string accessToken, List<string> endpointPolicies);
    }
}
