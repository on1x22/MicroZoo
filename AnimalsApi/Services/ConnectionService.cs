namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Keeps data about connection strings of other microservices
    /// </summary>
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _identityApiUrl;
        private readonly Uri _zookeepersApiUrl;

        /// <summary>
        /// Reads connection strings of other microservices from configuration files
        /// </summary>
        /// <param name="configuration"></param>
        public ConnectionService(IConfiguration configuration)
        {
            _identityApiUrl = new Uri(configuration["ConnectionStrings:IdentityApiRmq"]!);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]!);
        }

        public Uri IdentityApiUrl { get => _identityApiUrl; }
        public Uri ZookeepersApiUrl { get => _zookeepersApiUrl; }
    }
}
