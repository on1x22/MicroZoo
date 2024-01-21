using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Specialities;

namespace MicroZoo.Infrastructure.Services
{
    public interface IZookeeperApiService
    {
        Task<ZookeeperInfo> GetZookepeerInfoAsync(int id);
        Task<Person> GetPersonByIdFromPersonsApiAsync(int id);
        
        Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync();
        Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities);
        Task DeleteSpecialityAsync(int zookeeperId, int animalTypeId);
        Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id);
        Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom);
        Task<List<Job>> GetAllJobsOfZookeeperAsync(int id);
        Task AddJobAsync(int id, Job job);
        Task DeleteJobAsync(int id, int jobId);
        Task UpdateJobByZookeeperAsync(int id, Job job);
        Task FinishJobAsync(int id, Job job);

        /// <summary>
        /// Returns true, if one o more zokeepers with speciality exist in database
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="objectId"></param>
        /// <returns>True of false</returns>
        Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZokeepersWithSpecialityAreExistAsync(
            CheckType checkType, int objectId);

    }
}
