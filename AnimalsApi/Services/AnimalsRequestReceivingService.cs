using MassTransit;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public class AnimalsRequestReceivingService : IAnimalsRequestReceivingService
    {
        private readonly IServiceProvider _provider;
        private readonly IAnimalsApiService _animalsService;
        private readonly IConnectionService _connectionService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;

        public AnimalsRequestReceivingService(IServiceProvider provider, 
            IAnimalsApiService animalsService,
            IConnectionService connectionService,
            IResponsesReceiverFromRabbitMq receiver)
        {
            _provider = provider;
            _animalsService = animalsService;
            _connectionService = connectionService;
            _receiver = receiver;
        }

        public async Task<GetAnimalsResponse> GetAllAnimalsAsync() =>        
             await _animalsService.GetAllAnimalsAsync();
        
        public async Task<GetAnimalResponse> GetAnimalAsync(int animalId) =>        
            await _animalsService.GetAnimalAsync(animalId);
        
        public async Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto) =>
            await _animalsService.AddAnimalAsync(animalDto);        

        public async Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto) =>
            await _animalsService.UpdateAnimalAsync(animalId, animalDto);

        public async Task<GetAnimalResponse> DeleteAnimalAsync(int animalId) =>
            await _animalsService.DeleteAnimalAsync(animalId);

        public async Task<GetAnimalsResponse> GetAnimalsByTypesAsync(int[] animalTypesIds) =>
            await _animalsService.GetAnimalsByTypesAsync(animalTypesIds);

        

        /*private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(_rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }*/
    }
}
