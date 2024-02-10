
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class CheckZokeepersWithSpecialityAreExistRequest
    {
        public Guid OperationId { get; set; }
        public int ObjectId { get; set; }
        public CheckType CheckType { get; set; }


        public CheckZokeepersWithSpecialityAreExistRequest(CheckType checkType, int objectId)
        {
            OperationId = Guid.NewGuid();
            CheckType = checkType;            
            ObjectId = objectId;
        }
    }

    public enum CheckType
    {
        AnimalType,
        Person
    }
}
