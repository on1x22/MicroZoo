namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    /// <summary>
    /// Provides to send data about required person
    /// </summary>
    public class GetPersonRequest
    {
        /// <summary>
        /// Unique identifier of processing operation
        /// </summary>
        public Guid OperationId { get; set; }
        
        /// <summary>
        /// User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Access token for IdentityApi
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ResponsesReceiverFromRabbitMq"/> class 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accessToken"></param>
        public GetPersonRequest(int id, string accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AccessToken = accessToken;
        }
    }
}
