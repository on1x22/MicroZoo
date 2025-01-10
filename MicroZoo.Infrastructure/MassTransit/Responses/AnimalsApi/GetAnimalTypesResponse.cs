using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi
{
    public record GetAnimalTypesResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public List<AnimalType> AnimalTypes { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
