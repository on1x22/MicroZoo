using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class DeleteSpecialityRequest
    {
        public Guid OperationId { get; set; }
        public SpecialityDto SpecialityDto { get; set; }

        public DeleteSpecialityRequest(SpecialityDto specialityDto)
        {
            OperationId = Guid.NewGuid();
            SpecialityDto = specialityDto;
        }
    }
}
