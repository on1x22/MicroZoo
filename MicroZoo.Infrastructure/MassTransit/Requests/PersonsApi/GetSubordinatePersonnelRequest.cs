﻿namespace MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi
{
    public class GetSubordinatePersonnelRequest
    {
        public Guid OperationId { get; set; }
        public int PersonId { get; set; }

        public GetSubordinatePersonnelRequest(int personId)
        {
            OperationId = Guid.NewGuid();
            PersonId = personId;
        }
    }
}
