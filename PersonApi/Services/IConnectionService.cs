namespace MicroZoo.PersonsApi.Services
{
    public interface IConnectionService
    {
        Uri IdentityApiUrl { get; }
        Uri ZookeepersApiUrl { get; }
    }
}
