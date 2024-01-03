using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Models;
using MicroZoo.AnimalsApi.Repository;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Apis
{
    public class AnimalApi : IApi
    {
        private readonly IBusControl _busControl;
        private readonly IServiceProvider _provider;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/shopsQueue");

        public AnimalApi(IBusControl busControl, IServiceProvider provider)
        {
            _busControl = busControl;
            _provider = provider;
        }

        public void Register(WebApplication app)
        {
            app.MapGet("/", () => "Hello AnimalsApi!");

            app.MapGet("/animal/getall", GetAllAnimals);

            app.MapGet("animal/getanimalsbytypes", GetAnimalsByTypes);

            app.MapGet("animal/getanimalsbytypes2", GetAnimalsByTypes2);

            app.MapGet("animal/getallanimaltypes", GetAllAnimalTypes);

            app.MapGet("animal/getanimaltypesbyid", GetAnimalTypesByIds);
        }

        private async Task<IResult> GetAllAnimals(IAnimalsApiService service)
        {
            var response = await GetResponseRabbitTask<GetAllAnimalsRequest, GetAllAnimalsResponse> (new GetAllAnimalsRequest());
            return response.Animals is List<Animal> animals
                ? Results.Ok(animals)
                : Results.NoContent();
        }

        private async Task<IResult> GetAnimalsByTypes([FromBody] List<int> animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalsByTypes(animalTypeIds) is List<Animal> animals
            ? Results.Ok(animals)
            : Results.NotFound("Not all animal type Ids exist in database");

        internal static async Task<IResult> GetAllAnimalTypes(IAnimalRepository repository) =>
            await repository.GetAllAnimalTypes() is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NoContent();

        internal static async Task<IResult> GetAnimalsByTypes2([FromQuery] int[] animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalsByTypes2(animalTypeIds) is List<Animal> animals
            ? Results.Ok(animals)
            : Results.NotFound("Not all animal type Ids exist in database");


        internal static async Task<IResult> GetAnimalTypesByIds([FromQuery] int[] animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalTypesByIds(animalTypeIds) is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NotFound("Not all animal type Ids exist in database");

        private async Task<TOut> GetResponseRabbitTask<TIn, TOut>(TIn request)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(_rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
