using MicroZoo.ZookeeperCatalog.Models;

namespace MicroZoo.ZookeeperCatalog
{
    public interface IZookeeperCatalog
    {
        IEnumerable<Zookepeer> Get();

        Zookepeer Get(string name);
        Zookepeer Get(int id);

        IEnumerable<Zookepeer> GetZookeepers(string speciality);
    }
}
