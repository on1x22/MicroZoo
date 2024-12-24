namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class CheckAccessRequest
    {
        public Guid OperationId { get; set; }
        public string? AccessToken { get; set; }
        public List<string>? Policies { get; set; }

        public CheckAccessRequest(string accessToken, List<string> policies)
        {
            OperationId = Guid.NewGuid();
            AccessToken = accessToken;
            Policies = policies;
        }
    }
}
