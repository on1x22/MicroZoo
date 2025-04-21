namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to get list of animals with certain animal types types 
    /// by request receiving from RabbitMq
    /// </summary>
    public class GetAnimalsByTypesRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// List of animals types Ids
        /// </summary>
        public int[] AnimalTypesIds { get; set; }

        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetAnimalsByTypesRequest"/> class
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <param name="accessToken"></param>
        public GetAnimalsByTypesRequest(int[] animalTypesIds, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            AnimalTypesIds = animalTypesIds;
            //AccessToken = accessToken;
        }
    }
}
