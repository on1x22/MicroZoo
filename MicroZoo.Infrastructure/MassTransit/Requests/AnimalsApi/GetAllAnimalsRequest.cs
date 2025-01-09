namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class GetAllAnimalsRequest
    {
        public Guid OperationId { get; set; }
        public string AccessToken { get; }

        //public List<Animal> Animals { get; set; }

        public GetAllAnimalsRequest(string accessToken)
        {
            OperationId = Guid.NewGuid();
            AccessToken = accessToken;
        }
    }
}
