using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi
{
    public record GetAnimalResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public Animal Animal { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
