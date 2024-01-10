﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class GetAnimalsByTypesRequest
    {
        public Guid OperationId { get; set; }
        public int[] AnimalTypesIds { get; set; }

        public GetAnimalsByTypesRequest(int[] animalTypesIds)
        {
            OperationId = Guid.NewGuid();
            AnimalTypesIds = animalTypesIds;
        }
    }
}
