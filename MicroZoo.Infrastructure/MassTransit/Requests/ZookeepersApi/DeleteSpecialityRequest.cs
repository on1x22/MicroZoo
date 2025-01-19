using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class DeleteSpecialityRequest
    {
        public Guid OperationId { get; set; }
        public SpecialityDto SpecialityDto { get; set; }
        public string AccessToken { get; }

        public DeleteSpecialityRequest(SpecialityDto specialityDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            SpecialityDto = specialityDto;
            AccessToken = accessToken;
        }
    }
}
