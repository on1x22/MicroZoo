using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Xml.Linq;


namespace MicroZoo.ZookeepersApi.Repository
{
    public class ZookeeperRepository : IZookeeperRepository
    {
        private readonly ZookeeperDBContext _dBContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        public ZookeeperRepository(ZookeeperDBContext dBContext, IHttpClientFactory httpClientFactory)
        {
            _dBContext = dBContext;
            //_httpClientFactory = httpClientFactory;
            _client = httpClientFactory.CreateClient();
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
            //var client = _httpClientFactory.CreateClient();
            
            ZookeeperInfo zookeeperInfo = new ZookeeperInfo();
            

            
            zookeeperInfo.Adout = await GetByIdAsync(id);
            zookeeperInfo.Jobs = await _dBContext.Jobs.Where(j => j.ZookeeperId == id).ToListAsync();
            
            var specialitiesInId = await _dBContext.Specialities
                                            .Where(s => s.ZookeeperId == id)
                                            .Select(s => s.AnimalTypeId)
                                            .ToListAsync();

            string parametrs = "animalTypeIds=" + String.Join("&animalTypeIds=", specialitiesInId);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7284/animal/getanimaltypesbyid?" + parametrs)                
            };

            var response = await _client.SendAsync(request);
            zookeeperInfo.Specialities = await response.Content.ReadFromJsonAsync<List<AnimalType>>();

            var animalsRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7284/animal/getanimalsbytypes2?" + parametrs)
            };

            var animalsResponse = await _client.SendAsync(animalsRequest);
            var animals = await animalsResponse.Content.ReadFromJsonAsync<List<Animal>>();


            var observerAnimals = (from animal in animals
                                  join animalType in zookeeperInfo.Specialities
                                  on animal.AnimalTypeId equals animalType.Id
                                  select new ObservedAnimal
                                  {
                                      Id = animal.Id,
                                      Name = animal.Name,
                                      AnimalType = animalType.Description
                                  }).ToList();
            
            zookeeperInfo.ObservedAnimals = observerAnimals/*.ToList()*/;
            return zookeeperInfo;
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost:7206/person/{id}")
            };
            var response = await _client.SendAsync(request);
            
            return await response.Content.ReadFromJsonAsync<Person>();
        }

        public Task<List<string>> GetAllZookeperSpecialitiesAsync()
        {
            throw new NotImplementedException();
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
