using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;

namespace MicroZoo.PersonsApi.Services
{
    public interface IAuthorizationService
    {
        public Task<CheckAccessResponse> IsResourceAccessConfirmed(string accessToken, List<string> endpointPolicies);
    }
}
