using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.AuthService.Models;
using MicroZoo.AuthService.Policies;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.JwtConfiguration;

namespace MicroZoo.AuthService.Services
{
    /// <summary>
    /// Provides to send authorization requests to Identity microservice 
    /// and checks access to resource
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IResponsesReceiverFromRabbitMq _receiver;

        /// <summary>
        /// Initialize a new instance of <see cref="AuthorizationService"/> class 
        /// </summary>
        /// <param name="receiver"></param>
        public AuthorizationService(IResponsesReceiverFromRabbitMq receiver)
        {
            _receiver = receiver;
        }

        /// <summary>
        /// Check access to executing resource in IdentityApi
        /// </summary>
        /// <param name="httpRequest">Current http request for extracting access token</param>
        /// <param name="type">Current class</param>
        /// <param name="methodName">Name of current method</param>
        /// <param name="IdentityApiUrl">Url of IdentityApi</param>
        /// <returns></returns>
        public async Task<AccessResult> CheckAccessInIdentityApiAsync(HttpRequest httpRequest,
                                                                  Type type,
                                                                  string methodName,
                                                                  Uri IdentityApiUrl)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(httpRequest);
            var endpointPolicies = PoliciesValidator.GetPoliciesFromEndpoint(type, methodName);
            if (accessToken == null || (endpointPolicies == null || endpointPolicies.Count == 0))
                return new AccessResult(IsAccessAllowed: false, Result: new UnauthorizedResult());

            var accessResponse = await IsResourceAccessConfirmedAsync(IdentityApiUrl,
                                                                 accessToken,
                                                                 endpointPolicies);
            if (accessResponse.ErrorMessage != null)
                return new AccessResult(IsAccessAllowed: false,
                    Result: new BadRequestObjectResult(accessResponse.ErrorMessage));

            if (!accessResponse.IsAuthenticated)
                return new AccessResult(IsAccessAllowed: false, Result: new UnauthorizedResult());

            if (!accessResponse.IsAccessConfirmed)
                return new AccessResult(IsAccessAllowed: false, Result: new ForbidResult());

            return new AccessResult(IsAccessAllowed: true, Result: new OkResult());
        }

        private async Task<CheckAccessResponse> IsResourceAccessConfirmedAsync(Uri identityApiUri,
            string accessToken, List<string> endpointPolicies)
        {
            var accessResponse = await _receiver.GetResponseFromRabbitTask<CheckAccessRequest,
                CheckAccessResponse>(new CheckAccessRequest(accessToken, endpointPolicies),
                identityApiUri);

            if (accessResponse == null)
            {
                accessResponse = new CheckAccessResponse()
                { 
                    ErrorMessage = "Internal server error" 
                };
            }

            return accessResponse;
        }
    }
}
