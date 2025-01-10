using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class ChangeRelationBetweenZookeeperAndSpecialityRequest
    {
        public Guid OperationId { get; set; }
        public int RelationId { get; set; }
        public SpecialityDto SpecialityDto { get; set; }
        public string AccessToken { get; }

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
