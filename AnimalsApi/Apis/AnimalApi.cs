using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Models;
using MicroZoo.AnimalsApi.Repository;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Apis
{
    public class AnimalApi : IApi
    {
        public void Register(WebApplication app)
        {
            app.MapGet("/", () => "Hello AnimalsApi!");

            app.MapGet("animal/getanimalsbytypes", GetAnimalsByTypes);

            app.MapGet("animal/getanimalsbytypes2", GetAnimalsByTypes2);

            app.MapGet("animal/getallanimaltypes", GetAllAnimalTypes);

            app.MapGet("animal/getanimaltypesbyid", GetAnimalTypesByIds);
        }

        private async Task<IResult> GetAnimalsByTypes([FromBody] List<int> animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalsByTypes(animalTypeIds) is List<Animal> animals
            ? Results.Ok(animals)
            : Results.NotFound("Not all animal type Ids exist in database");

        private async Task<IResult> GetAllAnimalTypes(IAnimalRepository repository) =>
            await repository.GetAllAnimalTypes() is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NoContent();

        private async Task<IResult> GetAnimalsByTypes2([FromQuery] int[] animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalsByTypes2(animalTypeIds) is List<Animal> animals
            ? Results.Ok(animals)
            : Results.NotFound("Not all animal type Ids exist in database");


        private async Task<IResult> GetAnimalTypesByIds([FromQuery] int[] animalTypeIds, IAnimalRepository repository) =>
            await repository.GetAnimalTypesByIds(animalTypeIds) is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NotFound("Not all animal type Ids exist in database");
    }
}
