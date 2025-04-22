namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    /// <summary>
    /// Allows to get list of animals types with certain Ids types 
    /// by request receiving from RabbitMq
    /// </summary>
    public class GetAnimalTypesByIdsRequest : BaseRequest
    {
        //public Guid OperationId { get; set; }

        /// <summary>
        /// List of animals types Ids
        /// </summary>
        public int[] AnimalTypesIds { get; set; }

        //public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetAnimalTypesByIdsRequest"/> class
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <param name="accessToken"></param>
        public GetAnimalTypesByIdsRequest(int[] animalTypesIds, string accessToken) : base(accessToken)
        {
            OperationId = Guid.NewGuid();
            AnimalTypesIds = animalTypesIds;
            //AccessToken = accessToken;
        }
    }
}
