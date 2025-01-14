using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi
{
    public record GetPersonsResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public List<Person> Persons { get; set; }
        public string ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
