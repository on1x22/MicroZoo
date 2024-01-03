using MicroZoo.Infrastructure.Models.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class AddAnimalRequest
    {
        public Guid OperationId { get; set; }
        public Animal Animal { get; set; }

        public AddAnimalRequest(Animal animal)
        {
            OperationId = Guid.NewGuid();
            Animal = animal;
        }
    }
}
