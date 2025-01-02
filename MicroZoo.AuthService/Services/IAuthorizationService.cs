using Microsoft.AspNetCore.Http;
using MicroZoo.AuthService.Models;

namespace MicroZoo.AuthService.Services
{
    /// <summary>
    /// Provides to send authorization requests to Identity microservice 
    /// and checks access to resource
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Check access to executing resource in IdentityApi
        /// </summary>
        /// <param name="httpRequest">Received request</param>
        /// <param name="type">Type of object in which to access the resource</param>
        /// <param name="methodName">Name of method in which to access the resource</param>
        /// <param name="IdentityApiUrl">Url of IdentityApi</param>
        /// <returns></returns>
        Task<AccessResult> CheckAccessInIdentityApiAsync(HttpRequest httpRequest,
                                                                  Type type,
                                                                  string methodName,
                                                                  Uri IdentityApiUrl);

        /// <summary>
        /// Check access to executing resource in IdentityApi
        /// </summary>
        /// <param name="accessToken">Access token from http request</param>
        /// <param name="type">Current class</param>
        /// <param name="methodName">Name of current method</param>
        /// <param name="IdentityApiUrl">Url of IdentityApi</param>
        /// <returns></returns>
        Task<AccessResult> CheckAccessInIdentityApiAsync(string accessToken,
                                                                  Type type,
                                                                  string methodName,
                                                                  Uri IdentityApiUrl);
    }
}
