﻿using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;

namespace MicroZoo.AnimalsApi.Consumers
{
    public class GetAllAnimalTypesConsumer : IConsumer<GetAllAnimalTypesRequest>
    {
        private readonly IAnimalsApiService _service;

        public GetAllAnimalTypesConsumer(IAnimalsApiService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAllAnimalTypesRequest> context)
        {
            var response = await _service.GetAllAnimalTypesAsync();
            response.OperationId = context.Message.OperationId;

            await context.RespondAsync<GetAnimalTypesResponse>(response);
        }
    }
}
