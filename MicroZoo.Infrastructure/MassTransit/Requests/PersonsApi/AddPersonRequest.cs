using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    public class AddPersonRequest
    {
        public Guid OperationId { get; set; }
        public PersonDto PersonDto { get; set; }
        public string AccessToken { get; }

        public AddPersonRequest(PersonDto personDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            PersonDto = personDto;
            AccessToken = accessToken;
        }
    }
}
