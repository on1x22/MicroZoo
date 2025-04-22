namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to get list of all animals by request receiving from RabbitMq
    /// </summary>
    public class GetAllAnimalsRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }
        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetAllAnimalsRequest"/> class
        /// </summary>
        /// <param name="accessToken"></param>
        public GetAllAnimalsRequest(string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            //AccessToken = accessToken;
        }
    }
}
