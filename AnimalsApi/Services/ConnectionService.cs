namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Keeps data about connection strings of other microservices
    /// </summary>
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _identityApiUrl;

        /// <summary>
        /// Reads connection strings of other microservices from configuration files
        /// </summary>
        /// <param name="configuration"></param>
        public ConnectionService(IConfiguration configuration)
        {
            _identityApiUrl = new Uri(configuration["ConnectionStrings:IdentityApiRmq"]!);
        }

        public Uri IdentityApiUrl { get => _identityApiUrl; }
    }
}
