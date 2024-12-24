namespace MicroZoo.ZookeepersApi.Services
{
    public interface IAuthorizationService
    {
        public Task<bool> IsResourceAccessConfirmed(string accessToken, List<string> endpointPolicies);
    }
}
