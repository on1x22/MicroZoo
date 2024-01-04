using MicroZoo.Infrastructure.Models.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class UpdateAnimalRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }
        public Animal Animal { get; set; }

        public UpdateAnimalRequest(int id, Animal animal)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            Animal = animal;
        }
    }
}
