using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Specialities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public record GetJobsResponse
    {
        public Guid OperationId { get; set; }
        public List<Job> Jobs { get; set; }
        public string ErrorMessage { get; set; }
    }
}
