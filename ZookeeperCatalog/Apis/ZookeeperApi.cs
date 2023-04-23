using MicroZoo.ZookeeperCatalog.Models;
using MicroZoo.ZookeeperCatalog.Repository;

namespace MicroZoo.ZookeeperCatalog.Apis
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
        }

        private async Task<IResult> GetByName(string name, IZookeeperRepository repository) =>
            await repository.GetByNameAsync(name) is Zookepeer zookepeer
            ? Results.Ok(zookepeer) 
            : Results.NotFound();

        private async Task<IResult> GetById(int id, IZookeeperRepository repository) =>
            await repository.GetByIdAsync(id) is Zookepeer zookepeer
            ? Results.Ok(zookepeer)
            : Results.NotFound();

        private async Task<IResult> GetAll(IZookeeperRepository repository) =>
            await repository.GetAllAsync() is List<Zookepeer> all
            ? Results.Ok(all)
            : Results.NotFound();

        private async Task<IResult> GetBySpeciality(string speciality, IZookeeperRepository repository) =>
            await repository.GetBySpecialityAsync(speciality) is List<Zookepeer> zookeepres
            ? Results.Ok(zookeepres) 
            : Results.NotFound();
    }
}
