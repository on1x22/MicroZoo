using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class DeleteAnimalTypeRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }

        public DeleteAnimalTypeRequest(int id)
        {
            OperationId = Guid.NewGuid();
            Id = id;
        }
    }
}
