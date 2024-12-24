namespace MicroZoo.ZookeepersApi.Services
{
    public interface IConnectionService
    {
        Uri AnimalsApiUrl { get; /*set;*/ }
        Uri IdentityApiUrl { get; }
        Uri PersonsApiUrl { get; /*set;*/ }
        Uri ZookeepersApiUrl { get; /*set;*/ }
    }
}
