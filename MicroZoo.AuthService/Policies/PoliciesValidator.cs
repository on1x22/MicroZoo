using System.Reflection;

namespace MicroZoo.AuthService.Policies
{
    /// <summary>
    /// Checks policies identified in method
    /// </summary>
    public static class PoliciesValidator
    {
        /// <summary>
        /// Gets policies from <see cref="PolicyValidationAttribute"/> of method
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static List<string> GetPoliciesFromEndpoint(Type type, string methodName)
        {
            var methodInfo = type.GetMethod(methodName);

            var attributes = methodInfo!.GetCustomAttributes(typeof(PolicyValidationAttribute));
            var policies = (attributes as IEnumerable<PolicyValidationAttribute>)!.Select(p => p.Policy).ToList();

            if (policies == null || policies.Count == 0)
                return default!;

            return policies!;
        }
    }
}
