using MicroZoo.Infrastructure.Models.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi
{
    public record GetAnimalTypeResponse
    {
        public Guid OperationId { get; set; }
        public AnimalType AnimalType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
