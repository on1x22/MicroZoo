using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public record GetAllAnimalTypesResponse
    {
        public Guid OperationId { get; set; }
        public List<AnimalType> AnimalTypes { get; set; }
        public string ErrorMessage { get; set; }
    }
}
