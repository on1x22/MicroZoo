using MicroZoo.Infrastructure.Models.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class GetAllAnimalsRequest
    {
        public Guid OperationId { get; set; }
        //public List<Animal> Animals { get; set; }

        public GetAllAnimalsRequest()
        {
            OperationId = Guid.NewGuid();
        }
    }
}
