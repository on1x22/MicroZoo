using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class GetAnimalTypeRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }

        public GetAnimalTypeRequest(int id)
        {
            OperationId = Guid.NewGuid();
            Id = id;
        }
    }
}
