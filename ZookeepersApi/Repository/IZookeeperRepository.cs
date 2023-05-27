using MicroZoo.ZookeepersApi.Models;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.ZookeepersApi.Repository
{
    public interface IZookeeperRepository
    {
        // TODO: remove not actual methods
        Task<Zookeeper> GetByNameAsync(string name);
        Task<Person> GetByIdAsync(int id);
        Task<List<Zookeeper>> GetAllAsync();
        Task<List<Zookeeper>> GetBySpecialityAsync(string speciality);

        

        Task<ZookeeperInfo> GetZookepeerInfoAsync(int id);


        Task<List<AnimalType>> GetAllZookeperSpecialitiesAsync();
        Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities);
        Task DeleteSpecialityAsync(int zookeeperid, int animaltypeid);


        Task<List<Job>> GetAllJobsOfZookeeperAsync(int id);
        Task<Job> AddJobAsync(Job job);
        Task UpdateJobAsync(Job job);
    }
}
