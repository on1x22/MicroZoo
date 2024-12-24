using System.Reflection;

namespace MicroZoo.ZookeepersApi.Policies
{
    public static class PoliciesValidator
    {        
        public static List<string> GetPolicies(Type type, string methodName)
        {
            var methodInfo = type.GetMethod(methodName);
            
            var attributes = methodInfo.GetCustomAttributes(typeof(PolicyValidationAttribute));
            var policies = (attributes as IEnumerable<PolicyValidationAttribute>)!.Select(p => p.Policy).ToList();

            if (policies == null || policies.Count == 0) 
                return default!;
            
            return policies;
        }
    }
}
