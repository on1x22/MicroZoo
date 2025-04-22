namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to get animal by request receiving from RabbitMq
    /// </summary>
    public class GetAnimalRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// Animal's Id
        /// </summary>
        public int Id { get; set; }
        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetAnimalRequest"/> class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accessToken"></param>
        public GetAnimalRequest(int id, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            //AccessToken = accessToken;
        }
    }
}
