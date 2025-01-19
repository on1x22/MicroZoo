using MicroZoo.Infrastructure.Models.Specialities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public record GetSpecialitiesResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public List<Speciality> Specialities { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
