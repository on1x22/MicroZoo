using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.ZookeepersApi.Services
{
    public class __AuthorizationService : __IAuthorizationService
    {
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly IConnectionService _connectionService;

        public __AuthorizationService(IResponsesReceiverFromRabbitMq receiver, 
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
