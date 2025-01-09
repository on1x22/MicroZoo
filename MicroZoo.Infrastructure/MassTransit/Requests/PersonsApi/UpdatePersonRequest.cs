using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    public class UpdatePersonRequest
    {
        public Guid OperationId { get; set; }
        public int PersonId { get; set; }
        public PersonDto PersonDto { get; set; }
        public string AccessToken { get; }

        public UpdatePersonRequest(int personId, PersonDto personDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
            PersonDto = personDto;
            AccessToken = accessToken;
        }
    }
}
