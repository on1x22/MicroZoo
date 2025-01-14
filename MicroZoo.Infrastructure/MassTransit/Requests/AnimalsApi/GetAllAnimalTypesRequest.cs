namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class GetAllAnimalTypesRequest
    {
        public Guid OperationId { get; set; }
        public string AccessToken { get; }

        public GetAllAnimalTypesRequest(string accessToken)
        {
            OperationId = Guid.NewGuid();
            AccessToken = accessToken;
        }
    }
}
