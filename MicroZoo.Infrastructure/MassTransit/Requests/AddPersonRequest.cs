using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class AddPersonRequest
    {
        public Guid OperationId { get; set; }
        public PersonDto PersonDto { get; set; }

        public AddPersonRequest(PersonDto personDto)
        {
            OperationId = Guid.NewGuid();
            PersonDto = personDto;
        }
    }
}
