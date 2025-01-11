using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class UpdateAnimalRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }
        public AnimalDto AnimalDto { get; set; }
        public string AccessToken { get; }

        public UpdateAnimalRequest(int id, AnimalDto animalDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AnimalDto = animalDto;
            AccessToken = accessToken;
        }
    }
}
