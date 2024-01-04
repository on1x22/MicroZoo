using MicroZoo.Infrastructure.Models.Animals.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MassTransit.Requests
{
    public class GetAnimalRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }

        public GetAnimalRequest(int id)
        {
            OperationId = Guid.NewGuid();
            Id = id;
        }
    }
}
