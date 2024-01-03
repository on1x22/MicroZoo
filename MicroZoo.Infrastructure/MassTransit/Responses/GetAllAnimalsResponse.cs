using MicroZoo.Infrastructure.Models.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public record GetAllAnimalsResponse
    {
        public Guid OperationId { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
