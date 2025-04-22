using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to change relation between animal and zookeeper 
    /// by request receiving from RabbitMq
    /// </summary>
    public class ChangeRelationBetweenZookeeperAndSpecialityRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Animal and zookeeper relation's Id
        /// </summary>
        public int RelationId { get; set; }

        /// <summary>
        /// Information about new relation between animal and zookeeper
        /// </summary>
        public SpecialityDto SpecialityDto { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of 
        /// <see cref="ChangeRelationBetweenZookeeperAndSpecialityRequest"/> class
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="specialityDto"></param>
        /// <param name="accessToken"></param>
        public ChangeRelationBetweenZookeeperAndSpecialityRequest(int relationId, 
            SpecialityDto specialityDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            RelationId = relationId;
            SpecialityDto = specialityDto;
            AccessToken = accessToken;
        }
    }
}
