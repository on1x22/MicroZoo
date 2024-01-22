using MicroZoo.Infrastructure.Models.Specialities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
