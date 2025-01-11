
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class CheckZokeepersWithSpecialityAreExistRequest
    {
        public Guid OperationId { get; set; }
        public int ObjectId { get; set; }
        public string AccessToken { get; }
        public CheckType CheckType { get; set; }


        public CheckZokeepersWithSpecialityAreExistRequest(CheckType checkType, int objectId,
            string accessToken)
        {
            OperationId = Guid.NewGuid();
            CheckType = checkType;            
            ObjectId = objectId;
            AccessToken = accessToken;
        }
    }

    public enum CheckType
    {
        AnimalType,
        Person
    }
}
