using MicroZoo.ZookeeperCatalog.Models;

namespace MicroZoo.ZookeeperCatalog
{
    public interface IZookeeperCatalog
    {
        Zookepeer Get(string name);
        Zookepeer Get(int id);

        IEnumerable<Zookepeer> Get();
        IEnumerable<Zookepeer> GetZookeepers(string speciality);
    }
}
