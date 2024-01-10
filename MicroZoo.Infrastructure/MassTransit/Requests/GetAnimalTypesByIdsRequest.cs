using MicroZoo.Infrastructure.Models.Animals.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class GetAnimalTypesByIdsRequest
    {
        public Guid OperationId { get; set; }
        public int[] AnimalTypesIds { get; set; }

        public GetAnimalTypesByIdsRequest(int[] animalTypesIds)
        {
            OperationId = Guid.NewGuid();
            AnimalTypesIds = animalTypesIds;
        }
    }
}
