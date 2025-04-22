using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    /// <summary>
    /// Allows to add person by request receiving from RabbitMq
    /// </summary>
    public class AddPersonRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about person
        /// </summary>
        public PersonDto PersonDto { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="AddPersonRequest"/> class
        /// </summary>
        /// <param name="personDto"></param>
        /// <param name="accessToken"></param>
        public AddPersonRequest(PersonDto personDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            PersonDto = personDto;
            AccessToken = accessToken;
        }
    }
}
