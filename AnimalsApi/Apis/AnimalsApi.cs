using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Models;
using MicroZoo.AnimalsApi.Repository;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Apis
{
    public class AnimalsApi : IApi
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/animals-queue");

        public AnimalsApi( IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Register(WebApplication app)
        {
            //app.MapGet("/", () => "Hello AnimalsApi!");

            app.MapGet("/animals_old", GetAllAnimals)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapGet("/animals_old/{id}", GetAnimal)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapPost("animals_old", AddAnimal)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapPut("animals_old/{id}", UpdateAnimal)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapDelete("animals_old/{id}", DeleteAnimal)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapGet("animal/getanimalsbytypes", GetAnimalsByTypes)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapGet("animal/getanimalsbytypes2", GetAnimalsByTypes2)
                .WithOpenApi(operation => new(operation)
            {
                Deprecated = true
            });

            app.MapGet("animal/getallanimaltypes", GetAllAnimalTypes)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                });

            app.MapGet("animal/getanimaltypesbyid", GetAnimalTypesByIds)
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true
                })
                .WithDescription("This endpoint will be removed. /AnimalTypes/byIds should be used instead"); ;            
        }

        private async Task<IResult> GetAllAnimals(IAnimalsApiService service)
        {
            var response = await GetResponseFromRabbitTask<GetAllAnimalsRequest, GetAnimalsResponse> (new GetAllAnimalsRequest());
            return response.Animals is List<Animal> animals
                ? Results.Ok(animals)
                : Results.NoContent();
        }

        private async Task<IResult> GetAnimalsByTypes([FromBody] List<int> animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalsByTypes(animalTypeIds) is List<Animal> animals
            ? Results.Ok(animals)
            : Results.NotFound("Not all animal type Ids exist in database");

        internal static async Task<IResult> GetAllAnimalTypes(IAnimalRepository repository) =>
            await repository.GetAllAnimalTypesAsync() is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NoContent();

        internal static async Task<IResult> GetAnimalsByTypes2([FromQuery] int[] animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalsByTypesAsync(animalTypeIds) is List<Animal> animals
            ? Results.Ok(animals)
            : Results.NotFound("Not all animal type Ids exist in database");


        internal static async Task<IResult> GetAnimalTypesByIds([FromQuery] int[] animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalTypesByIdsAsync(animalTypeIds) is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NotFound("Not all animal type Ids exist in database");

        internal async Task<IResult> GetAnimal(int id)
        {
            var response = await GetResponseFromRabbitTask<GetAnimalRequest, GetAnimalResponse>(new GetAnimalRequest(id));

            return response.Animal != null
                ? Results.Ok(response.Animal)
                : Results.NotFound(response.ErrorMessage);
        }

        internal async Task<IResult> AddAnimal([FromBody] AnimalDto animalDto)
        {
            var response = await GetResponseFromRabbitTask<AddAnimalRequest, GetAnimalResponse>(new AddAnimalRequest(animalDto));
            return response.Animal != null
                ? Results.Ok(response.Animal)
                : Results.BadRequest(response.ErrorMessage);
        }

        internal async Task<IResult> UpdateAnimal(int id, [FromBody] AnimalDto animalDto)
        {
            var response = await GetResponseFromRabbitTask<UpdateAnimalRequest, GetAnimalResponse>(new UpdateAnimalRequest(id, animalDto));
            return response.Animal != null
                ? Results.Ok(response.Animal)
                : Results.BadRequest(response.ErrorMessage);
        }

        internal async Task<IResult> DeleteAnimal(int id)
        {
            var response = await GetResponseFromRabbitTask<DeleteAnimalRequest, GetAnimalResponse>(new DeleteAnimalRequest(id));
            return response.Animal != null
                ? Results.Ok(response.Animal)
                : Results.NotFound(response.ErrorMessage);
        }

        private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request)
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
