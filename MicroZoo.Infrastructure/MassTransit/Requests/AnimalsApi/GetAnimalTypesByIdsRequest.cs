namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class GetAnimalTypesByIdsRequest
    {
        public Guid OperationId { get; set; }
        public int[] AnimalTypesIds { get; set; }
        public string AccessToken { get; }

        public GetAnimalTypesByIdsRequest(int[] animalTypesIds, string accessToken)
        {
            OperationId = Guid.NewGuid();
            AnimalTypesIds = animalTypesIds;
            AccessToken = accessToken;
        }
    }
}
