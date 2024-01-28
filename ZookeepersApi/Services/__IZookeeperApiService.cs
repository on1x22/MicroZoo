using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface __IZookeeperApiService
    {
        Task<ZookeeperInfo> GetZookepeerInfoAsync(int id);
        
        Task<Person> GetPersonByIdFromPersonsApiAsync(int id);
        
        
        
        
        
        






        [Obsolete("Old solution")]
        Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync();

        [Obsolete("Old solution")]
        Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities);

        [Obsolete("Old solution")]
        Task DeleteSpecialityAsync(int zookeeperId, int animalTypeId);



        [Obsolete("Old solution")]
        Task<List<Job>> GetAllJobsOfZookeeperAsync(int id);

        [Obsolete("Old solution")]
        Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id);

        [Obsolete("Old solution")]
        Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom);

        [Obsolete("Old solution")]
        Task AddJobAsync(int id, Job job);

        [Obsolete("Old solution")]
        Task UpdateJobByZookeeperAsync(int id, Job job);

        [Obsolete("Old solution")]
        Task FinishJobAsync(int id, Job job);

        [Obsolete("Old solution")]
        Task DeleteJobAsync(int id, int jobId);
    }
}
