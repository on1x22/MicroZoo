
namespace MicroZoo.ZookeepersApi.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _animalsApiUrl;
        private readonly Uri _identityApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public ConnectionService(/*Uri animalsApiUrl, Uri personsApiUrl, Uri zookeepersApiUrl*/
            IConfiguration configuration)
        {
            _animalsApiUrl = /*animalsApiUrl;*/new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _identityApiUrl = new Uri(configuration["ConnectionStrings:IdentityApiRmq"]);
            _personsApiUrl = /*personsApiUrl;*/new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = /*zookeepersApiUrl;*/new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        public Uri AnimalsApiUrl { get => _animalsApiUrl; /*set => _animalsApiUrl = value;*/ }
        public Uri IdentityApiUrl { get => _identityApiUrl; }
        public Uri PersonsApiUrl { get => _personsApiUrl; }
        public Uri ZookeepersApiUrl { get => _zookeepersApiUrl; }
    }
}
