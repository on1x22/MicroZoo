using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.Infrastructure.Models.Persons;
using Newtonsoft.Json;

namespace MicroZoo.ZookeepersApi.Apis
{
    public class ZookeeperApi : IApi
    {
        public void Register(WebApplication app)
        {
            app.MapGet("/", () => "Hello ZookeeperCatalog!");

            app.MapGet("/zookeeper/name/{name}", GetByName);

            app.MapGet("/zookeeper/id/{id}", GetById);

            app.MapGet("/zookeeper", GetAll);

            app.MapGet("/zookeeper/speciality/{speciality}", GetBySpeciality);

            app.MapGet("/zookeeper/{id}", GetZookepeerInfoAsync);
        }

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


        private async Task<IResult> GetZookepeerInfoAsync(int id, IZookeeperRepository repository) =>
            await repository.GetZookepeerInfoAsync(id) is ZookeeperInfo zookeeper
            ? Results.Ok(zookeeper)
            : Results.NotFound();
    }
}
