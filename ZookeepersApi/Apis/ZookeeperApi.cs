using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.Infrastructure.Models.Persons;
using Newtonsoft.Json;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.ZookeepersApi.Apis
{
    public class ZookeeperApi : IApi
    {
        public void Register(WebApplication app)
        {
            app.MapGet("/", () => "Hello ZookeepersApi!")
                .ExcludeFromDescription();

            app.MapGet("/zookeeper/name/{name}", GetByName)
                .ExcludeFromDescription();

            app.MapGet("/zookeeper/id/{id}", GetById)
                .ExcludeFromDescription();

            app.MapGet("/zookeeper", GetAll)
                .ExcludeFromDescription();

            app.MapGet("/zookeeper/speciality/{speciality}", GetBySpeciality)
                .ExcludeFromDescription();

            // new functionality:
            app.MapGet("/zookeeper/{id}", GetZookepeerInfoAsync)
                .WithTags("Index");

            app.MapGet("/zookeeper/speciality/all", GetAllZookeperSpecialitiesAsync)
                .WithTags("Speciality");

            app.MapPut("/zookeeper/speciality", ChangeSpecialitiesAsync)
                .WithTags("Speciality");

            app.MapDelete("/zookeeper/{zookeeperid}/speciality/{animaltypeid}", DeleteSpecialityAsync)
                .WithTags("Speciality");
        }
        #region
        private async Task<IResult> GetByName(string name, IZookeeperRepository repository) =>
            await repository.GetByNameAsync(name) is Zookeeper zookepeer
            ? Results.Ok(zookepeer) 
            : Results.NotFound();

        private async Task<IResult> GetById(int id, IZookeeperRepository repository) =>
            await repository.GetByIdAsync(id) is Person zookepeer
            ? Results.Ok(zookepeer)
            : Results.NotFound();

        private async Task<IResult> GetAll(IZookeeperRepository repository) =>
            await repository.GetAllAsync() is List<Zookeeper> all
            ? Results.Ok(all)
            : Results.NotFound();

        private async Task<IResult> GetBySpeciality(string speciality, IZookeeperRepository repository) =>
            await repository.GetBySpecialityAsync(speciality) is List<Zookeeper> zookeepres
            ? Results.Ok(zookeepres) 
            : Results.NotFound();
        #endregion

        // new functionality:

        private async Task<IResult> GetZookepeerInfoAsync(int id, IZookeeperRepository repository) =>
            await repository.GetZookepeerInfoAsync(id) is ZookeeperInfo zookeeper
            ? Results.Ok(zookeeper)
            : Results.NotFound("Zookeeper is not found");

        private async Task<IResult> GetAllZookeperSpecialitiesAsync(IZookeeperRepository repository) =>
            await repository.GetAllZookeperSpecialitiesAsync() is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NotFound();

        private async Task<IResult> ChangeSpecialitiesAsync(List<Speciality> animalTypes, 
                                                       IZookeeperRepository repository)
        {
           await repository.ChangeSpecialitiesAsync(animalTypes);

            var id = animalTypes.FirstOrDefault().ZookeeperId;            
            return await GetZookepeerInfoAsync(id, repository);
        }

        private async Task<IResult> DeleteSpecialityAsync(int zookeeperId, int animaltypeId, 
            IZookeeperRepository repository)
        { 
            await repository.DeleteSpecialityAsync(zookeeperId, animaltypeId);
            return await GetZookepeerInfoAsync(zookeeperId, repository);
        }

    }
}
