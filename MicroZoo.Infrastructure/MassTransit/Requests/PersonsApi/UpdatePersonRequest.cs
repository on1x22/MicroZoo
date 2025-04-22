using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    /// <summary>
    /// Allows to update information about person by request receiving from RabbitMq
    /// </summary>
    public class UpdatePersonRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Id of the person being updated
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// New information about the person who will be updated
        /// </summary>
        public PersonDto PersonDto { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="UpdatePersonRequest"/> class
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="personDto"></param>
        /// <param name="accessToken"></param>
        public UpdatePersonRequest(int personId, PersonDto personDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
            PersonDto = personDto;
            AccessToken = accessToken;
        }
    }
}
