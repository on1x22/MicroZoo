namespace MicroZoo.PersonsApi.Services
{
    /// <summary>
    /// Reads connection strings of other microservices from configuration files
    /// </summary>
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _identityApiUrl;
        private readonly Uri _zookeepersApiUrl;

        /// <summary>
        /// Initializes a new instance of <see cref="ConnectionService"/> class 
        /// </summary>
        /// <param name="configuration"></param>
        public ConnectionService(IConfiguration configuration)
        {
            _identityApiUrl = new Uri(configuration["ConnectionStrings:IdentityApiRmq"]!);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]!);
        }

        /// <summary>
        /// Connection string to IdentityApi
        /// </summary>
        public Uri IdentityApiUrl { get => _identityApiUrl; }

        /// <summary>
        /// Connection string to ZookeepersApi
        /// </summary>
        public Uri ZookeepersApiUrl { get => _zookeepersApiUrl; }
    }
}
