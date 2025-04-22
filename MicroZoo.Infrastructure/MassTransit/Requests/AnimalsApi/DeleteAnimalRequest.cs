namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to delete animal by request receiving from RabbitMq
    /// </summary>
    public class DeleteAnimalRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// Animal's Id
        /// </summary>
        public int Id { get; set; }

        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="DeleteAnimalRequest"/> class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accessToken"></param>
        public DeleteAnimalRequest(int id, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            //AccessToken = accessToken;
        }
    }
}
