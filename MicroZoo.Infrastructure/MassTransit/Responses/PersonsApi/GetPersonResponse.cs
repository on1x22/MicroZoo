using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi
{
    public record GetPersonResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public Person Person { get; set; }
        public string? ErrorMessage { get; set; }
        //public IActionResult ActionResult {  get; set; }
        //public ResponseError ResponseError { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
