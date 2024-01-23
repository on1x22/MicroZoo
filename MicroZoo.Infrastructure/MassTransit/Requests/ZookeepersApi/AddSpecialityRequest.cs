using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class AddSpecialityRequest
    {
        public Guid OperationId { get; set; }
        public SpecialityDto SpecialityDto { get; set; }

        public AddSpecialityRequest(SpecialityDto specialityDto)
        {
            OperationId = Guid.NewGuid();
            SpecialityDto = specialityDto;
        }
    }
}
