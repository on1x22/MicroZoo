using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeeperCatalog.DBContext;
using MicroZoo.ZookeeperCatalog.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MicroZoo.ZookeeperCatalog.Repository
{
    public class ZookeeperRepository : IZookeeperRepository
    {
        // Temporal solution
        /*private List<Zookepeer> _zookepeers = new List<Zookepeer>
        {
            new Zookepeer { Id = 1, Name = "Sam", Specialities = new List<string>{ "Tigers" } },
            new Zookepeer { Id = 2, Name = "Bob", Specialities = new List<string>{ "Bears", "Lamas"} },
            new Zookepeer { Id = 3, Name = "Tom", Specialities = new List<string>{ "Birds" }}
        };*/

        private readonly ZookeeperDBContext _dBContext;

        public ZookeeperRepository(ZookeeperDBContext dBContext)
        {
                _dBContext = dBContext;
        }

        public async Task<Zookepeer> GetByNameAsync(string name) =>
            await _dBContext.Zookepeers.FirstOrDefaultAsync(z => z.Name == name);
        

        public async Task<Zookepeer> GetByIdAsync(int id) =>
            await _dBContext.Zookepeers.FirstOrDefaultAsync(z => z.Id == id);
        
        //[HttpGet]
        public async Task<List<Zookepeer>> GetAllAsync() =>
            await _dBContext.Zookepeers.ToListAsync();
        
        public async Task<List<Zookepeer>> GetBySpecialityAsync(string speciality) =>
            await _dBContext.Zookepeers.Where(z => z.Specialities.Contains(speciality)).ToListAsync();
        
    }
}
