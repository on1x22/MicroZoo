using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.AuthService.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        //private readonly IConnectionService _connectionService;

        public AuthorizationService(IResponsesReceiverFromRabbitMq receiver/*, 
            IConnectionService connectionService*/)
        {
            _receiver = receiver;
            //_connectionService = connectionService;
        }

        public async Task<CheckAccessResponse> IsResourceAccessConfirmed(Uri identityApiUri,
            string accessToken, List<string> endpointPolicies)
        {
            var accessResponse = await _receiver.GetResponseFromRabbitTask<CheckAccessRequest,
                CheckAccessResponse>(new CheckAccessRequest(accessToken, endpointPolicies),
                identityApiUri);

            if (accessResponse == null)
            {
                accessResponse.ErrorMessage = "Internal server error";
            }

            return accessResponse;
        }
    }
}
