
namespace ManagersApi.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _identityApiUrl;

        public ConnectionService(IConfiguration configuration)
        {
            _identityApiUrl = new Uri(configuration["ConnectionStrings:IdentityApiRmq"]!);               
        }

        public Uri IdentityApi { get =>  _identityApiUrl; }
    }
}
