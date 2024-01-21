using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.Infrastructure.Models.Persons.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class ChangeRelationBetweenZookeeperAndSpecialityRequest
    {
        public Guid OperationId { get; set; }
        public int RelationId { get; set; }
        public SpecialityDto SpecialityDto { get; set; }

        public ChangeRelationBetweenZookeeperAndSpecialityRequest(int relationId, 
            SpecialityDto specialityDto)
        {
            OperationId = Guid.NewGuid();
            RelationId = relationId;
            SpecialityDto = specialityDto;
        }
    }
}
