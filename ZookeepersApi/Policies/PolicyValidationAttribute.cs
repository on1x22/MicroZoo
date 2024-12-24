namespace MicroZoo.ZookeepersApi.Policies
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PolicyValidationAttribute : Attribute
    {
        public string Policy {  get; set; }
        /*public string Policy { get; }

        public PolicyValidationAttribute(string policy)
        {
            Policy = policy;
        }*/
    }
}
