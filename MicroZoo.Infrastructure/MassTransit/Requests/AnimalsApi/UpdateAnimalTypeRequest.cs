using MicroZoo.Infrastructure.Models.Animals.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class UpdateAnimalTypeRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }
        public AnimalTypeDto AnimalTypeDto { get; set; }

        public UpdateAnimalTypeRequest(int id, AnimalTypeDto animalTypeDto)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AnimalTypeDto = animalTypeDto;
        }
    }
}
