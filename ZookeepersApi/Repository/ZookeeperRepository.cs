using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace MicroZoo.ZookeepersApi.Repository
{
    public class ZookeeperRepository : IZookeeperRepository
    {
        private readonly ZookeeperDBContext _dBContext;
        private readonly RequestHelper _requestHelper;

        public ZookeeperRepository(ZookeeperDBContext dBContext, RequestHelper requestHelper)
        {
            _dBContext = dBContext;
            _requestHelper = requestHelper;
        }

        // TODO: remove not actual methods
        public async Task<Zookeeper> GetByNameAsync(string name) =>
            await _dBContext.Zookepeers.FirstOrDefaultAsync(z => z.Name == name);
        
       
                
        public async Task<List<Zookeeper>> GetAllAsync() =>
            await _dBContext.Zookepeers.ToListAsync();
        
        public async Task<List<Zookeeper>> GetBySpecialityAsync(string speciality) =>
            await _dBContext.Zookepeers.Where(z => z.Specialities.Contains(speciality)).ToListAsync();





        public async Task<ZookeeperInfo> GetZookepeerInfoAsync(int id)
        {            
            ZookeeperInfo zookeeperInfo = new ZookeeperInfo();
                        
            zookeeperInfo.Adout = await GetByIdAsync(id);
            zookeeperInfo.Jobs = await _dBContext.Jobs.Where(j => j.ZookeeperId == id).ToListAsync();
            
            var specialitiesInId = await _dBContext.Specialities
                                            .Where(s => s.ZookeeperId == id)
                                            .Select(s => s.AnimalTypeId)
                                            .ToListAsync();

            string parametrs = "animalTypeIds=" + String.Join("&animalTypeIds=", specialitiesInId);
            string requestString = "https://localhost:7284/animal/getanimaltypesbyid?" + parametrs;
            
            zookeeperInfo.Specialities = await _requestHelper
                .GetResponseAsync<List<AnimalType>>(method: HttpMethod.Get,
                                                    requestUri: requestString);
                        
            requestString = "https://localhost:7284/animal/getanimalsbytypes2?" + parametrs;
            var animals = await _requestHelper.GetResponseAsync<List<Animal>>(method: HttpMethod.Get,
                                                                              requestUri: requestString);

            var observerAnimals = (from animal in animals
                                  join animalType in zookeeperInfo.Specialities
                                  on animal.AnimalTypeId equals animalType.Id
                                  select new ObservedAnimal
                                  {
                                      Id = animal.Id,
                                      Name = animal.Name,
                                      AnimalType = animalType.Description
                                  }).ToList();
            
            zookeeperInfo.ObservedAnimals = observerAnimals;
            return zookeeperInfo;
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            string requestString = $"https://localhost:7206/person/{id}";

            return await _requestHelper.GetResponseAsync<Person>(method: HttpMethod.Get,
                                                                 requestUri: requestString);
        }

        public async Task<List<AnimalType>> GetAllZookeperSpecialitiesAsync()
        {
            string requestString = "https://localhost:7284/animal/getallanimaltypes";
            return await _requestHelper.GetResponseAsync<List<AnimalType>>(method: HttpMethod.Get,
                                                                     requestUri: requestString);
        }

        public Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities)
        {
            throw new NotImplementedException();
        }

        public Task<List<Job>> GetAllJobsOfZookeeperAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Job> AddJobAsync(Job job)
        {
            throw new NotImplementedException();
        }

        public Task UpdateJobAsync(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
