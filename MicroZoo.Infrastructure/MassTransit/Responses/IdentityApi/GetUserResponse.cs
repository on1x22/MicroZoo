using MicroZoo.Infrastructure.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public record GetUserResponse
    {
        public Guid OperationId { get; set; }
        public User? User { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
