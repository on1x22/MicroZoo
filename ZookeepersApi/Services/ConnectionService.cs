
namespace MicroZoo.ZookeepersApi.Services
{
    /// <summary>
    /// Keeps data about connection strings of other microservices
    /// </summary>
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _animalsApiUrl;
        private readonly Uri _identityApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        /// <summary>
        /// Reads connection strings of other microservices from configuration files
        /// </summary>
        /// <param name="configuration"></param>
        public ConnectionService(IConfiguration configuration)
        {
            _animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]!);
            _identityApiUrl = new Uri(configuration["ConnectionStrings:IdentityApiRmq"]!);
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]!);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]!);
        }

        public Uri AnimalsApiUrl { get => _animalsApiUrl; }
        public Uri IdentityApiUrl { get => _identityApiUrl; }
        public Uri PersonsApiUrl { get => _personsApiUrl; }
        public Uri ZookeepersApiUrl { get => _zookeepersApiUrl; }
    }
}
