namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to delete animal type by request receiving from RabbitMq
    /// </summary>
    public class DeleteAnimalTypeRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// Animal's type Id
        /// </summary>
        public int Id { get; set; }

        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="DeleteAnimalTypeRequest"/> class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accessToken"></param>
        public DeleteAnimalTypeRequest(int id, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            //AccessToken = accessToken;
        }
    }
}
