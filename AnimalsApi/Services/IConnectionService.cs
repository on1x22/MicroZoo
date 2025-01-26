namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Keeps data about connection strings of other microservices
    /// </summary>
    public interface IConnectionService
    {
        /// <summary>
        /// Connection string to IdentityApi
        /// </summary>
        Uri IdentityApiUrl { get; }

        /// <summary>
        /// Connection string to ZookeepersApi
        /// </summary>
        Uri ZookeepersApiUrl { get; }
    }
}
