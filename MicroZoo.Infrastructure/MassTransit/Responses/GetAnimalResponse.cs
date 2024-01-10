using MicroZoo.Infrastructure.Models.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public record GetAnimalResponse
    {
        public Guid OperationId { get; set; }
        public Animal Animal { get; set; }
        public string ErrorMessage { get; set; }
    }
}
