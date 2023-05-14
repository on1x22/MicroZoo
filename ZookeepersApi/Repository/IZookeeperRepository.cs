using MicroZoo.ZookeepersApi.Models;
using MicroZoo.Infrastructure.Models.Persons;

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


        Task<List<string>> GetAllZookeperSpecialitiesAsync();
        Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities);


        Task<List<Job>> GetAllJobsOfZookeeperAsync(int id);
        Task<Job> AddJobAsync(Job job);
        Task UpdateJobAsync(Job job);
    }
}
