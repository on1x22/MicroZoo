using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Services;
using MicroZoo.Infrastructure.Models.Jobs;

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
                .WithTags("IndexOld");

            app.MapGet("/zookeeper/speciality/all", GetAllZookeperSpecialitiesAsync)
                .WithTags("SpecialityOld")
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true,
                    Summary = "Moved to [GET] /Specialities"
                });

            app.MapPut("/zookeeper/speciality", ChangeSpecialitiesAsync)
                .WithTags("SpecialityOld")
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true,
                    Summary = "Improved and moved to [PUT] /Specialities/{relationId}"
                });

            app.MapDelete("/zookeeper/{zookeeperid}/speciality/{animaltypeid}", DeleteSpecialityAsync)
                .WithTags("SpecialityOld")
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true,
                    Summary = "Moved to [DELETE] /Specialities"
                });

            app.MapGet("/zookeeper/{zookeeperid}/jobs/current", GetCurrentJobsOfZookeeperAsync)
                .WithTags("JobsOld")
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true,
                    Summary = "Moved to [GET] /Jobs/{zookeeperId}/current"
                });

            app.MapGet("/zookeeper/{zookeeperid}/jobs/from/{datetimefrom}", GetJobsOfZookeeperFromAsync)
                .WithTags("JobsOld");

            app.MapGet("/zookeeper/{zookeeperid}/jobs/all", GetAllJobsOfZookeeperAsync)
                .WithTags("JobsOld")
                .WithOpenApi(operation => new(operation)
                {
                    Deprecated = true,
                    Summary = "Moved to [GET] /Jobs/{zookeeperId}"
                });

            app.MapPost("/zookeeper/{zookeeperid}/jobs", AddJobAsync)
                .WithTags("JobsOld");

            app.MapDelete("/zookeeper/{zookeeperid}/jobs/{jobid}", DeleteJobAsync)
                .WithTags("JobsOld");

            app.MapPut("/zookeeper/{zookeeperid}/jobs", UpdateJobByZookeeperAsync)
                .WithTags("JobsOld");

            app.MapPut("/zookeeper/{zookeeperid}/jobs/finish", FinishJobAsync)
                .WithTags("JobsOld");
        }
        #region
        private async Task<IResult> GetByName(string name, __IZookeeperRepository repository) =>
            await repository.GetByNameAsync(name) is Zookeeper zookepeer
            ? Results.Ok(zookepeer) 
            : Results.NotFound();

        internal static async Task<IResult> GetById(int id, __IZookeeperRepository repository,
                                                    __IZookeeperApiService service) =>
            await service.GetPersonByIdFromPersonsApiAsync(id) is Person zookepeer
            ? Results.Ok(zookepeer)
            : Results.NotFound();

        private async Task<IResult> GetAll(__IZookeeperRepository repository) =>
            await repository.GetAllAsync() is List<Zookeeper> all
            ? Results.Ok(all)
            : Results.NotFound();

        private async Task<IResult> GetBySpeciality(string speciality, __IZookeeperRepository repository) =>
            await repository.GetBySpecialityAsync(speciality) is List<Zookeeper> zookeepres
            ? Results.Ok(zookeepres) 
            : Results.NotFound();
        #endregion

        // new functionality:

        private async Task<IResult> GetZookepeerInfoAsync(int id,
                                                          __IZookeeperApiService service) =>
            await service.GetZookepeerInfoAsync(id) is ZookeeperInfo zookeeper
            ? Results.Ok(zookeeper)
            : Results.NotFound("Zookeeper is not found");

        
        

        // TODO: Need to figure out how to display the time
        private async Task<IResult> GetJobsOfZookeeperFromAsync(int zookeeperid,
                                                                DateTime datetimefrom,
                                                                __IZookeeperApiService service) =>
            await service.GetJobsOfZookeeperFromAsync(zookeeperid, datetimefrom) 
            is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        // TODO: Need to figure out how to display the time
        

        private async Task<IResult> AddJobAsync(int zookeeperid, Job job,
                                                __IZookeeperApiService service)
        {
            await service.AddJobAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }
            
        private async Task<IResult> DeleteJobAsync(int zookeeperid, int jobid,
                                                   __IZookeeperApiService service)
        {
            await service.DeleteJobAsync(zookeeperid, jobid);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }

        private async Task<IResult> UpdateJobByZookeeperAsync(int zookeeperid, Job job,
                                                              __IZookeeperApiService service)
        {
            await service.UpdateJobByZookeeperAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }

        private async Task<IResult> FinishJobAsync(int zookeeperid, Job job,
                                                   __IZookeeperApiService service)
        {
            await service.FinishJobAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, service);
        }







        [Obsolete("Please use GetAllSpecialities() in SpecialitiesController instead.")]
        private async Task<IResult> GetAllZookeperSpecialitiesAsync(__IZookeeperApiService service) =>
            await service.GetAllAnimalTypesFromAnimalsApiAsync() is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NotFound();

        [Obsolete("Please use ChangeRelationBetweenZookeeperAndSpeciality() in SpecialitiesController instead.")]
        private async Task<IResult> ChangeSpecialitiesAsync(List<Speciality> animalTypes,
                                                            __IZookeeperApiService service)
        {
            await service.ChangeSpecialitiesAsync(animalTypes);

            return animalTypes.FirstOrDefault().ZookeeperId is int id
                 ? await GetZookepeerInfoAsync(id, service)
                 : Results.BadRequest(id);
        }

        [Obsolete("Please use DeleteSpeciality() in SpecialitiesController instead.")]
        private async Task<IResult> DeleteSpecialityAsync(int zookeeperId, int animaltypeId,
                                                          __IZookeeperApiService service)
        {
            await service.DeleteSpecialityAsync(zookeeperId, animaltypeId);
            return await GetZookepeerInfoAsync(zookeeperId, service);
        }



        [Obsolete("Please use GetAllJobsOfZookeeper() in JobsController instead.")]
        private async Task<IResult> GetAllJobsOfZookeeperAsync(int zookeeperid,
                                                               __IZookeeperApiService service) =>
            await service.GetAllJobsOfZookeeperAsync(zookeeperid) is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        [Obsolete("Please use GetCurrentJobsOfZookeeper() in JobsController instead.")]
        private async Task<IResult> GetCurrentJobsOfZookeeperAsync(int zookeeperId,
                                                                   __IZookeeperApiService service) =>
            await service.GetCurrentJobsOfZookeeperAsync(zookeeperId) is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();
    }
}
