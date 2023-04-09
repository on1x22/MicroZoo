using MicroZoo.ZookeeperCatalog.Models;

namespace MicroZoo.ZookeeperCatalog
{
    public interface IZookeeperCatalog
    {
        Zookepeer GetByName(string name);
        Zookepeer GetById(int id);

        IEnumerable<Zookepeer> GetAll();
        IEnumerable<Zookepeer> GetZookeepersSpeciality(string speciality);
    }
}
