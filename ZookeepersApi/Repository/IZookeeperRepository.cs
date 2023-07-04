using MicroZoo.ZookeepersApi.Models;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.ZookeepersApi.Repository
{
    public interface IZookeeperRepository
    {
        // TODO: remove not actual methods
        Task<Zookeeper> GetByNameAsync(string name);
        Task<Person> GetPersonByIdFromPersonsApiAsync(string requestString);
        Task<List<Zookeeper>> GetAllAsync();
        Task<List<Zookeeper>> GetBySpecialityAsync(string speciality);

        

        Task<ZookeeperInfo> GetZookepeerInfoAsync(int id);
        Task<List<Job>> GetJobsById(int id);
        Task<List<int>> GetSpecialitiesIdByPersonId(int id);
        Task<List<AnimalType>> GetAnimalTypesByIds(string requestString);
        Task<List<Animal>> GetAnimalsByAnimalTypesIds(string requestString);
        List<ObservedAnimal> GetObservedAnimals(List<Animal> animals, List<AnimalType> animalTypes);


        Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync(string requestString);
        Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities);
        Task DeleteSpecialityAsync(int zookeeperid, int animaltypeid);


        Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id);
        Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom);
        Task<List<Job>> GetAllJobsOfZookeeperAsync(int id);
        Task AddJobAsync(int id, Job job);        
        Task DeleteJobAsync(int id, int jobId);
        Task UpdateJobByZookeeperAsync(int id, Job job);
        Task FinishJobAsync(int id, Job job);
    }
}
