namespace MicroZoo.ZookeepersApi.Services
{
    public interface IConnectionService
    {
        Uri AnimalsApiUrl { get; }
        Uri IdentityApiUrl { get; }
        Uri PersonsApiUrl { get; }
        Uri ZookeepersApiUrl { get; }
    }
}
