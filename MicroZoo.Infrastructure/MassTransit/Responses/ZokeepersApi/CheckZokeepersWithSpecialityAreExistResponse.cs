namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public record CheckZokeepersWithSpecialityAreExistResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public bool IsThereZookeeperWithThisSpeciality { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode {  get; set; }
    }
}
