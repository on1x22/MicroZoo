using MicroZoo.ZookeeperCatalog;

namespace MicroZoo.ZookeeperCatalog.Models
{
    public class ZookeeperCatalog : IZookeeperCatalog
    {
        // Temporal solution
        private List<Zookepeer> _zookepeers = new List<Zookepeer>
        {
            new Zookepeer { Id = 1, Name = "Sam", Speciality = new List<string>{ "Tigers" } },
            new Zookepeer { Id = 2, Name = "Bob", Speciality = new List<string>{ "Bears", "Lamas"} },
            new Zookepeer { Id = 3, Name = "Tom", Speciality = new List<string>{ "birds" }}
        };

        public Zookepeer Get (string name)
        {
            return _zookepeers.FirstOrDefault(z => z.Name == name);
        }

        public Zookepeer Get(int id)
        {
            return _zookepeers.FirstOrDefault(z => z.Id == id);
        }

        public IEnumerable<Zookepeer> Get()
        {
            return _zookepeers;
        }

        public IEnumerable<Zookepeer> GetZookepeers(string speciality)
        {
            var ddd = _zookepeers[0].Specialities.Where(s => s == speciality);
            return _zookepeers.Select(z => z.Specialities.Where(s => s == speciality));
        }
    }
}
