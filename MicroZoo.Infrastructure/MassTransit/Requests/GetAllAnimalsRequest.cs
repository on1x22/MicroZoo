﻿
namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class GetAllAnimalsRequest
    {
        public Guid OperationId { get; set; }
        //public List<Animal> Animals { get; set; }

        public GetAllAnimalsRequest()
        {
            OperationId = Guid.NewGuid();
        }
    }
}
