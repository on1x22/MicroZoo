
namespace MicroZoo.ZookeepersApi.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public ConnectionService(/*Uri animalsApiUrl, Uri personsApiUrl, Uri zookeepersApiUrl*/
            IConfiguration configuration)
        {
            _animalsApiUrl = /*animalsApiUrl;*/new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _personsApiUrl = /*personsApiUrl;*/new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = /*zookeepersApiUrl;*/new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        public Uri AnimalsApiUrl { get => _animalsApiUrl; /*set => _animalsApiUrl = value;*/ }
        public Uri PersonsApiUrl { get => _personsApiUrl; }
        public Uri ZookeepersApiUrl { get => _zookeepersApiUrl; }
    }
}
