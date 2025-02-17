﻿using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi
{
    public record GetAnimalsResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public List<Animal> Animals { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
