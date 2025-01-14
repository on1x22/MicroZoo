namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class DeleteAnimalRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }
        public string AccessToken { get; }

        public DeleteAnimalRequest(int id, string accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AccessToken = accessToken;
        }
    }
}
