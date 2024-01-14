using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class UpdatePersonRequest
    {
        public Guid OperationId { get; set; }
        public int PersonId { get; set; }
        public PersonDto PersonDto { get; set; }

        public UpdatePersonRequest(int personId, PersonDto personDto)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
            PersonDto = personDto;
        }
    }
}
