using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public record GetPersonsResponse
    {
        public Guid OperationId { get; set; }
        public List<Person> Persons { get; set; }
        public string ErrorMessage { get; set; }
    }
}
