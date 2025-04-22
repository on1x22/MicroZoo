namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    /// <summary>
    /// Provides information about confirmation of user access 
    /// to certain resource for response through RabbitMq
    /// </summary>
    public class CheckAccessResponse
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about confirmation of user access 
        /// to certain resource
        /// </summary>
        public bool IsAccessConfirmed { get; set; }

        /// <summary>
        /// information about authentication of user
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
