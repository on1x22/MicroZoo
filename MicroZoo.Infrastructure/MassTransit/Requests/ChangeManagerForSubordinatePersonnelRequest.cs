using MicroZoo.Infrastructure.Models.Persons.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests
{
    public class ChangeManagerForSubordinatePersonnelRequest
    {
        public Guid OperationId { get; set; }
        public int CurrentManagerId { get; set; }
        public int NewManagerId { get; set; }

        public ChangeManagerForSubordinatePersonnelRequest(int currentManagerId,
                                                           int newManagerId)
        {
            OperationId = Guid.NewGuid();
            CurrentManagerId = currentManagerId;
            NewManagerId = newManagerId;
        }
    }
}
