﻿using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.Infrastructure.Models.Persons;
using Newtonsoft.Json;
using MicroZoo.Infrastructure.Models.Animals;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.ZookeepersApi.Services;

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

        private async Task<IResult> GetZookepeerInfoAsync(int id, /*IZookeeperRepository repository*/
                                                          IZookeeperApiService service) =>
            await service.GetZookepeerInfoAsync(id) is ZookeeperInfo zookeeper
            ? Results.Ok(zookeeper)
            : Results.NotFound("Zookeeper is not found");

        private async Task<IResult> GetAllZookeperSpecialitiesAsync(IZookeeperRepository repository,
                                                                    IZookeeperApiService service) =>
            await service.GetAllAnimalTypesFromAnimalsApiAsync() is List<AnimalType> animalTypes
            ? Results.Ok(animalTypes)
            : Results.NotFound();

        private async Task<IResult> ChangeSpecialitiesAsync(List<Speciality> animalTypes, 
                                                            IZookeeperRepository repository,
                                                            IZookeeperApiService service)
        {
           await repository.ChangeSpecialitiesAsync(animalTypes);

            var id = animalTypes.FirstOrDefault().ZookeeperId;            
            return await GetZookepeerInfoAsync(id, /*repository*/service);
        }

        private async Task<IResult> DeleteSpecialityAsync(int zookeeperId, int animaltypeId, 
                                                          IZookeeperRepository repository,
                                                          IZookeeperApiService service)
        { 
            await repository.DeleteSpecialityAsync(zookeeperId, animaltypeId);
            return await GetZookepeerInfoAsync(zookeeperId, /*repository*/service);
        }

        // TODO: Need to figure out how to display the time
        private async Task<IResult> GetCurrentJobsOfZookeeperAsync(int zookeeperId, 
                                                                   IZookeeperRepository repository) =>
            await repository.GetCurrentJobsOfZookeeperAsync(zookeeperId) is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        // TODO: Need to figure out how to display the time
        private async Task<IResult> GetJobsOfZookeeperFromAsync(int zookeeperid,
                                                                DateTime datetimefrom,
                                                                IZookeeperRepository repository) =>
            await repository.GetJobsOfZookeeperFromAsync(zookeeperid, datetimefrom) 
            is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        // TODO: Need to figure out how to display the time
        private async Task<IResult> GetAllJobsOfZookeeperAsync(int zookeeperid,
                                                               IZookeeperRepository repository) =>
            await repository.GetAllJobsOfZookeeperAsync(zookeeperid) is List<Job> jobs
            ? Results.Ok(jobs)
            : Results.NotFound();

        private async Task<IResult> AddJobAsync(int zookeeperid, /*[FromBody]*/ Job job,
                                                IZookeeperRepository repository,
                                                IZookeeperApiService service)
        {
            await repository.AddJobAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, /*repository*/service);
        }
            
        private async Task<IResult> DeleteJobAsync(int zookeeperid, int jobid,
                                                   IZookeeperRepository repository,
                                                   IZookeeperApiService service)
        {
            await repository.DeleteJobAsync(zookeeperid, jobid);
            return await GetZookepeerInfoAsync(zookeeperid, /*repository*/service);
        }

        private async Task<IResult> UpdateJobByZookeeperAsync(int zookeeperid, Job job,
                                                              IZookeeperRepository repository,
                                                              IZookeeperApiService service)
        {
            await repository.UpdateJobByZookeeperAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, /*repository*/service);
        }

        private async Task<IResult> FinishJobAsync(int zookeeperid, Job job,
                                                   IZookeeperRepository repository,
                                                   IZookeeperApiService service)
        {
            await repository.FinishJobAsync(zookeeperid, job);
            return await GetZookepeerInfoAsync(zookeeperid, /*repository*/service);
        }
    }
}
