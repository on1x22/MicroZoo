﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public record CheckZokeepersWithSpecialityAreExistResponse
    {
        public Guid OperationId { get; set; }
        public bool IsThereZookeeperWithThisSpeciality { get; set; }
        public string ErrorMessage { get; set; }
    }
}
