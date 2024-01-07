
namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class GetAllAnimalTypesRequest
    {
        public Guid OperationId { get; set; }

        public GetAllAnimalTypesRequest()
        {
            OperationId = Guid.NewGuid();
        }
    }
}
