using MicroZoo.Infrastructure.Models.Animals.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class AddAnimalTypeRequest
    {
        public Guid OperationId { get; set; }
        public AnimalTypeDto AnimalTypeDto { get; set; }
        public string AccessToken { get; }

        public AddAnimalTypeRequest(AnimalTypeDto animalTypeDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            AnimalTypeDto = animalTypeDto;
            AccessToken = accessToken;
        }
    }
}
