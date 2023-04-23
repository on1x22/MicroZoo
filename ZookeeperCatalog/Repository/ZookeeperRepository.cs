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
        private readonly ZookeeperDBContext _dBContext;

        public ZookeeperRepository(ZookeeperDBContext dBContext)
        {
                _dBContext = dBContext;
        }

        public async Task<Zookepeer> GetByNameAsync(string name) =>
            await _dBContext.Zookepeers.FirstOrDefaultAsync(z => z.Name == name);
        

        public async Task<Zookepeer> GetByIdAsync(int id) =>
            await _dBContext.Zookepeers.FirstOrDefaultAsync(z => z.Id == id);
             
        public async Task<List<Zookepeer>> GetAllAsync() =>
            await _dBContext.Zookepeers.ToListAsync();
        
        public async Task<List<Zookepeer>> GetBySpecialityAsync(string speciality) =>
            await _dBContext.Zookepeers.Where(z => z.Specialities.Contains(speciality)).ToListAsync();
        
    }
}
