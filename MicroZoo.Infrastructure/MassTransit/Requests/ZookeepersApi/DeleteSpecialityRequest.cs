using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to delete speciality by request receiving from RabbitMq
    /// </summary>
    public class DeleteSpecialityRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about speciality
        /// </summary>
        public SpecialityDto SpecialityDto { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="DeleteSpecialityRequest"/> class
        /// </summary>
        /// <param name="specialityDto"></param>
        /// <param name="accessToken"></param>
        public DeleteSpecialityRequest(SpecialityDto specialityDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            SpecialityDto = specialityDto;
            AccessToken = accessToken;
        }
    }
}
