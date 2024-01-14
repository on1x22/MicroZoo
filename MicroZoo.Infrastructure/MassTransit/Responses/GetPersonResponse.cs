using MicroZoo.Infrastructure.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses
{
    public record GetPersonResponse
    {
        public Guid OperationId { get; set; }
        public Person Person { get; set; }
        public string ErrorMessage { get; set; }
    }
}
