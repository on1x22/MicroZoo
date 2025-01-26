using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class UpdateAnimalTypeRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }
        public AnimalTypeDto AnimalTypeDto { get; set; }
        public string AccessToken { get; }

        public UpdateAnimalTypeRequest(int id, AnimalTypeDto animalTypeDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AnimalTypeDto = animalTypeDto;
            AccessToken = accessToken;
        }
    }
}
