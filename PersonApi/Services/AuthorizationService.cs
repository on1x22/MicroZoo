using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.MassTransit;

namespace MicroZoo.PersonsApi.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly IConnectionService _connectionService;

        public AuthorizationService(IResponsesReceiverFromRabbitMq receiver,
            IConnectionService connectionService)
        {
            _receiver = receiver;
            _connectionService = connectionService;
        }

        public async Task<CheckAccessResponse> IsResourceAccessConfirmed(string accessToken, List<string> endpointPolicies)
        {
            var accessResponse = await _receiver.GetResponseFromRabbitTask<CheckAccessRequest,
                CheckAccessResponse>(new CheckAccessRequest(accessToken, endpointPolicies),
                _connectionService.IdentityApiUrl);

            if (accessResponse == null)
            {
                accessResponse.ErrorMessage = "Internal server error";
            }

            return accessResponse;
        }
    }
}
