using Microsoft.AspNetCore.Http;
using MicroZoo.AuthService.Models;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.AuthService.Services
{
    public interface IAuthorizationService
    {
        Task<AccessResult> CheckAccessInIdentityApiAsync(HttpRequest httpRequest,
                                                                  Type type,
                                                                  string methodName,
                                                                  Uri IdentityApiUrl);
        //public Task<CheckAccessResponse> IsResourceAccessConfirmedAsync(Uri identityApiUri, 
        //    string accessToken, List<string> endpointPolicies);
    }
}
