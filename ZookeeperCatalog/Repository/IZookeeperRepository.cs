using MicroZoo.ZookeeperCatalog.Models;

namespace MicroZoo.ZookeeperCatalog.Repository
{
    public interface IZookeeperRepository
    {
        Task<Zookepeer> GetByNameAsync(string name);
        Task<Zookepeer> GetByIdAsync(int id);

        Task<List<Zookepeer>> GetAllAsync();
        Task<List<Zookepeer>> GetBySpecialityAsync(string speciality);
    }
}
