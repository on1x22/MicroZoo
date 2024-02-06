
namespace MicroZoo.ZookeepersApi.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public ConnectionService(Uri animalsApiUrl, Uri personsApiUrl, Uri zookeepersApiUrl)
        {
            _animalsApiUrl = animalsApiUrl;
            _personsApiUrl = personsApiUrl;
            _zookeepersApiUrl = zookeepersApiUrl;
        }

        public Uri AnimalsApiUrl { get => _animalsApiUrl; /*set => _animalsApiUrl = value;*/ }
        public Uri PersonsApiUrl { get => _personsApiUrl; }
        public Uri ZookeepersApiUrl { get => _zookeepersApiUrl; }
    }
}
