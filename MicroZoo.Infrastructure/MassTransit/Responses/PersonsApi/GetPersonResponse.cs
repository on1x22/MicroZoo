using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi
{
    public record GetPersonResponse
    {
        public Guid OperationId { get; set; }
        public Person Person { get; set; }

        [Obsolete("Should be moved to ActionResult")]
        public string ErrorMessage { get; set; }
        public IActionResult ActionResult {  get; set; }
    }
}
