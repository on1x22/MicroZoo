using Microsoft.AspNetCore.Mvc;
using MicroZoo.ZookeeperCatalog;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MicroZoo.ZookeeperCatalog.Models
{
    public class ZookeeperCatalog : IZookeeperCatalog
    {
        // Temporal solution
        private List<Zookepeer> _zookepeers = new List<Zookepeer>
        {
            new Zookepeer { Id = 1, Name = "Sam", Specialities = new List<string>{ "Tigers" } },
            new Zookepeer { Id = 2, Name = "Bob", Specialities = new List<string>{ "Bears", "Lamas"} },
            new Zookepeer { Id = 3, Name = "Tom", Specialities = new List<string>{ "Birds" }}
        };

        public Zookepeer GetByName(string name)
        {
            return _zookepeers.FirstOrDefault(z => z.Name == name);
        }

        public Zookepeer GetById(int id)
        {
            return _zookepeers.FirstOrDefault(z => z.Id == id);
        }
        [HttpGet]
        public IEnumerable<Zookepeer> GetAll()
        {            
            return _zookepeers;
        }

        public IEnumerable<Zookepeer> GetZookeepersSpeciality(string speciality)
        {
            var zSpec = _zookepeers.Where(z => z.Specialities.Contains(speciality));
            return zSpec;
        }
    }
}
