using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class AddAnimalRequest
    {
        public Guid OperationId { get; set; }
        public AnimalDto AnimalDto { get; set; }

        public AddAnimalRequest(AnimalDto animalDto)
        {
            OperationId = Guid.NewGuid();
            AnimalDto = animalDto;
        }
    }
}
