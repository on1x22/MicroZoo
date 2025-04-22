namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    /// <summary>
    /// Provides information about existing relations animal type with any zookeeper
    /// for response through RabbitMq
    /// </summary>
    public record CheckZokeepersWithSpecialityAreExistResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about existing relations animal type with any zookeeper
        /// </summary>
        public bool IsThereZookeeperWithThisSpeciality { get; set; }

        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Http code of occurred error
        /// </summary>
        public ErrorCodes? ErrorCode {  get; set; }
    }
}
