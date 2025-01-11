using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi
{
    public record GetAnimalTypeResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public AnimalType AnimalType { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
