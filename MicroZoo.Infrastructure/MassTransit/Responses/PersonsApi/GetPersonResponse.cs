using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi
{
    public record GetPersonResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public Person Person { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
