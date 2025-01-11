using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class AddAnimalRequest
    {
        public Guid OperationId { get; set; }
        public AnimalDto AnimalDto { get; set; }
        public string AccessToken { get; }

        public AddAnimalRequest(AnimalDto animalDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            AnimalDto = animalDto;
            AccessToken = accessToken;
        }
    }
}
