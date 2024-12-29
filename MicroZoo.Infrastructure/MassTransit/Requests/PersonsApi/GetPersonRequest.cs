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
        /// Initializes a new instance of <see cref="ResponsesReceiverFromRabbitMq"/> class 
        /// </summary>
        /// <param name="id"></param>
        public GetPersonRequest(int id)
        {
            OperationId = Guid.NewGuid();
            Id = id;
        }
    }
}
