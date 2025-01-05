using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi
{
    public class GetAnimalTypeRequest
    {
        public Guid OperationId { get; set; }
        public int Id { get; set; }
        public string AccessToken { get; }

        public GetAnimalTypeRequest(int id, string accessToken)
        {
            OperationId = Guid.NewGuid();
            Id = id;
            AccessToken = accessToken;
        }
    }
}
