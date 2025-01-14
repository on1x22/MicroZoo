using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class AddSpecialityRequest
    {
        public Guid OperationId { get; set; }
        public SpecialityDto SpecialityDto { get; set; }
        public string AccessToken { get; }

        public AddSpecialityRequest(SpecialityDto specialityDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            SpecialityDto = specialityDto;
            AccessToken = accessToken;
        }
    }
}
