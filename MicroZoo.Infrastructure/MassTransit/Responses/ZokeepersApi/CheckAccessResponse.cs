namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public class CheckAccessResponse
    {
        public Guid OperationId { get; set; }
        public bool IsAccessConfirmed {  get; set; } 
        public bool IsAuthenticated { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
