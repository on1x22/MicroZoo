namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to get list of all animal types by request receiving from RabbitMq
    /// </summary>
    public class GetAllAnimalTypesRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }
        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetAllAnimalTypesRequest"/> class
        /// </summary>
        /// <param name="accessToken"></param>
        public GetAllAnimalTypesRequest(string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            //AccessToken = accessToken;
        }
    }
}
