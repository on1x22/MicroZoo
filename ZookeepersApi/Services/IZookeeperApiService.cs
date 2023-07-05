using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.Services
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
    }
}
