
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to check that relation between animal and zookeeper are exist
    /// </summary>
    public class CheckZokeepersWithSpecialityAreExistRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Id of animal type or zookeeper
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Type of object that check
        /// </summary>
        public CheckType CheckType { get; set; }

        /// <summary>
        /// Initialize a new instance of 
        /// <see cref="CheckZokeepersWithSpecialityAreExistRequest"/> class
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="objectId"></param>
        /// <param name="accessToken"></param>
        public CheckZokeepersWithSpecialityAreExistRequest(CheckType checkType, int objectId,
            string accessToken)
        {
            OperationId = Guid.NewGuid();
            CheckType = checkType;            
            ObjectId = objectId;
            AccessToken = accessToken;
        }
    }

    /// <summary>
    /// List of types of selectable object
    /// </summary>
    public enum CheckType
    {
        /// <summary>
        /// Selectable object is animal type
        /// </summary>
        AnimalType,

        /// <summary>
        /// Selectable object is person (zookeeper)
        /// </summary>
        Person
    }
}
