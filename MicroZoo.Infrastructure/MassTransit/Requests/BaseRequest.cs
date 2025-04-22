namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    /// <summary>
    /// Base request for RabbitMq
    /// </summary>
    public abstract class BaseRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string? AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="BaseRequest"/> class
        /// </summary>
        /// <param name="accessToken"></param>
        protected BaseRequest(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
