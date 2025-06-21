using Microsoft.AspNetCore.Authorization;

namespace MicroZoo.IdentityApi.Tests
{
    internal static class AuthorizeAttributeHandler
    {
        internal static bool HasAuthorizeAttribute<T>(string methodName)
        {
            var methodInfo = typeof(T).GetMethod(methodName);
            return methodInfo!.GetCustomAttributes(typeof(AuthorizeAttribute), true).Length != 0;
        }

        internal static bool HasAuthorizeAttributeWithPolicy<T>(string methodName, string policy)
        {
            var methodInfo = typeof(T).GetMethod(methodName);
            var authorizeAttr = methodInfo!.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .FirstOrDefault() as AuthorizeAttribute;
            return authorizeAttr?.Policy == policy;
        }
    }
}
