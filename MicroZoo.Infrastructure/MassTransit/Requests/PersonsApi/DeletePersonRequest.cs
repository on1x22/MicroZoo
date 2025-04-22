namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    /// <summary>
    /// Allows to delete person by request receiving from RabbitMq
    /// </summary>
    public class DeletePersonRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Id of the person to be deleted
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="DeletePersonRequest"/> class
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="accessToken"></param>
        public DeletePersonRequest(int personId, string accessToken)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
            AccessToken = accessToken;
        }
    }
}
