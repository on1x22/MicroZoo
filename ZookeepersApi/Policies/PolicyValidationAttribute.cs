namespace MicroZoo.ZookeepersApi.Policies
{
    /// <summary>
    /// Identifies policy which checks in IdentityApi
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PolicyValidationAttribute : Attribute
    {
        /// <summary>
        /// Creates policy
        /// </summary>
        public string? Policy {  get; set; }        
    }
}
