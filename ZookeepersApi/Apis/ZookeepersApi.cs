using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Apis
{
    public class ZookeepersApi : IApi
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
                .WithTags("Index")
                .WithDescription("ewgwegq");

            app.MapGet("/zookeeper/speciality/all", GetAllZookeperSpecialitiesAsync)
                .WithTags("Speciality")
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true,
                    Summary = "Moved to [GET] /Specialities"
                });

            // TODO: Now works only INSERT operation/ DELETE not supports
            app.MapPut("/zookeeper/speciality", ChangeSpecialitiesAsync)
                .WithTags("Speciality");

            app.MapDelete("/zookeeper/{zookeeperid}/speciality/{animaltypeid}", DeleteSpecialityAsync)
                .WithTags("Speciality");

            app.MapGet("/zookeeper/{zookeeperid}/jobs/current", GetCurrentJobsOfZookeeperAsync)
                .WithTags("Jobs");

            app.MapGet("/zookeeper/{zookeeperid}/jobs/from/{datetimefrom}", GetJobsOfZookeeperFromAsync)
                .WithTags("Jobs");

            app.MapGet("/zookeeper/{zookeeperid}/jobs/all", GetAllJobsOfZookeeperAsync)
                .WithTags("Jobs");

            app.MapPost("/zookeeper/{zookeeperid}/jobs", AddJobAsync)
                .WithTags("Jobs");

            app.MapDelete("/zookeeper/{zookeeperid}/jobs/{jobid}", DeleteJobAsync)
                .WithTags("Jobs");

            app.MapPut("/zookeeper/{zookeeperid}/jobs", UpdateJobByZookeeperAsync)
                .WithTags("Jobs");

            app.MapPut("/zookeeper/{zookeeperid}/jobs/finish", FinishJobAsync)
                .WithTags("Jobs");
        }
        #region
        private async Task<IResult> GetByName(string name, IZookeeperRepository repository) =>
            await repository.GetByNameAsync(name) is Zookeeper zookepeer
            ? Results.Ok(zookepeer) 
            : Results.NotFound();

        internal static async Task<IResult> GetById(int id, IZookeeperRepository repository,
                                                    IZookeeperApiService service) =>
            await service.GetPersonByIdFromPersonsApiAsync(id) is Person zookepeer
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

        private async Task<IResult> GetZookepeerInfoAsync(int id,
                                                          IZookeeperApiService service) =>
            await service.GetZookepeerInfoAsync(id) is ZookeeperInfo zookeeper
            ? Results.Ok(zookeeper)
            : Results.NotFound("Zookeeper is not found");

        private async Task<IResult> GetAllZookeperSpecialitiesAsync(IZookeeperApiService service) =>
            await service.GetAllAnimalTypesFromAnimalsApiAsync() is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NotFound();

        private async Task<IResult> ChangeSpecialitiesAsync(List<Speciality> animalTypes,
                                                            IZookeeperApiService service)
        {
           await service.ChangeSpecialitiesAsync(animalTypes);
            
           return animalTypes.FirstOrDefault().ZookeeperId is int id
                ? await GetZookepeerInfoAsync(id, service)
                : Results.BadRequest(id);
        }

        private async Task<IResult> DeleteSpecialityAsync(int zookeeperId, int animaltypeId,
                                                          IZookeeperApiService service)
        { 
            await service.DeleteSpecialityAsync(zookeeperId, animaltypeId);
            return await GetZookepeerInfoAsync(zookeeperId, service);
        }

        // TODO: Need to figure out how to display the time
        private async Task<IResult> GetCurrentJobsOfZookeeperAsync(int zookeeperId,
                                                                   IZookeeperApiService service) =>
            await service.GetCurrentJobsOfZookeeperAsync(zookeeperId) is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        // TODO: Need to figure out how to display the time
        private async Task<IResult> GetJobsOfZookeeperFromAsync(int zookeeperid,
                                                                DateTime datetimefrom,
                                                                IZookeeperApiService service) =>
            await service.GetJobsOfZookeeperFromAsync(zookeeperid, datetimefrom) 
            is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        // TODO: Need to figure out how to display the time
        private async Task<IResult> GetAllJobsOfZookeeperAsync(int zookeeperid,
                                                               IZookeeperApiService service) =>
            await service.GetAllJobsOfZookeeperAsync(zookeeperid) is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        private async Task<IResult> AddJobAsync(int zookeeperid, Job job,
                                                IZookeeperApiService service)
        {
            await service.AddJobAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }
            
        private async Task<IResult> DeleteJobAsync(int zookeeperid, int jobid,
                                                   IZookeeperApiService service)
        {
            await service.DeleteJobAsync(zookeeperid, jobid);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }

        private async Task<IResult> UpdateJobByZookeeperAsync(int zookeeperid, Job job,
                                                              IZookeeperApiService service)
        {
            await service.UpdateJobByZookeeperAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }

        private async Task<IResult> FinishJobAsync(int zookeeperid, Job job,
                                                   IZookeeperApiService service)
        {
            await service.FinishJobAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }
    }
}
