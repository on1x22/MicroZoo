﻿using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IZookeeperApiService
    {
        Task<ZookeeperInfo> GetZookepeerInfoAsync(int id);
        
        Task<Person> GetPersonByIdFromPersonsApiAsync(int id);

        Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id);
        
        Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom);
        
        Task<List<Job>> GetAllJobsOfZookeeperAsync(int id);
        
        Task AddJobAsync(int id, Job job);
       
        Task DeleteJobAsync(int id, int jobId);
        
        Task UpdateJobByZookeeperAsync(int id, Job job);
        
        Task FinishJobAsync(int id, Job job);






        [Obsolete("Old solution")]
        Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync();

        [Obsolete("Old solution")]
        Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities);

        [Obsolete("Old solution")]
        Task DeleteSpecialityAsync(int zookeeperId, int animalTypeId);
    }
}
