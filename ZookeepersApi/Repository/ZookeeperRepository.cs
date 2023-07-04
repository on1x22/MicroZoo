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
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace MicroZoo.ZookeepersApi.Repository
{
    public class ZookeeperRepository : IZookeeperRepository
    {
        private readonly ZookeeperDBContext _dBContext;
        private readonly RequestHelper _requestHelper;
        private readonly string _personsApi;
        private readonly string _animalsApi;

        public ZookeeperRepository(ZookeeperDBContext dBContext, RequestHelper requestHelper)
        {
            _dBContext = dBContext;
            _requestHelper = requestHelper;
            _personsApi = Environment.GetEnvironmentVariable("PERSONS_API");
            _animalsApi = Environment.GetEnvironmentVariable("ANIMALS_API");
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
                        
            //zookeeperInfo.Adout = await GetPersonByIdFromPersonsApiAsync(id);
            if (zookeeperInfo.Adout == null)
                return default;

            zookeeperInfo.Jobs = await GetJobsById(id);
                /*await _dBContext.Jobs.Where(j => j.ZookeeperId == id).ToListAsync();*/
            
            var idOfSpecialities = await GetSpecialitiesIdByPersonId(id);
                                            /*_dBContext.Specialities
                                            .Where(s => s.ZookeeperId == id)
                                            .Select(s => s.AnimalTypeId)
                                            .ToListAsync();*/

            if (idOfSpecialities != null && idOfSpecialities.Count > 0)
            {
                string parameters = "animalTypeIds=" + String.Join("&animalTypeIds=", idOfSpecialities);
                string requestString = $"{_animalsApi}/animal/getanimaltypesbyid?" + parameters;

                zookeeperInfo.Specialities = await GetAnimalTypesByIds(requestString);
                    /*_requestHelper
                    .GetResponseAsync<List<AnimalType>>(method: HttpMethod.Get,
                                                        requestUri: requestString);*/

                if (zookeeperInfo.Specialities.Count == 0)
                    return zookeeperInfo;

                requestString = $"{_animalsApi}/animal/getanimalsbytypes2?" + parameters;
                var animals = await GetAnimalsByAnimalTypesIds(requestString);
                /*_requestHelper.GetResponseAsync<List<Animal>>(method: HttpMethod.Get,
                                                                              requestUri: requestString);*/

                /*var observedAnimals = (from animal in animals
                                       join animalType in zookeeperInfo.Specialities
                                       on animal.AnimalTypeId equals animalType.Id
                                       select new ObservedAnimal
                                       {
                                           Id = animal.Id,
                                           Name = animal.Name,
                                           AnimalType = animalType.Description
                                       }).ToList();*/

                zookeeperInfo.ObservedAnimals = GetObservedAnimals(animals, zookeeperInfo.Specialities);
                                                //observedAnimals;
            }
            return zookeeperInfo;
        }

        public async Task<Person> GetPersonByIdFromPersonsApiAsync(string requestString)
        {
            //string requestString = $"{_personsApi}/person/{id}";

            return await _requestHelper.GetResponseAsync<Person>(method: HttpMethod.Get,
                                                                 requestUri: requestString);
        }

        public async Task<List<Job>> GetJobsById(int id) =>        
            await _dBContext.Jobs.Where(j => j.ZookeeperId == id).ToListAsync();
        

        public async Task<List<int>> GetSpecialitiesIdByPersonId(int id) =>        
            await _dBContext.Specialities.Where(s => s.ZookeeperId == id)
                                         .Select(s => s.AnimalTypeId)
                                         .ToListAsync();
        
        public async Task<List<AnimalType>> GetAnimalTypesByIds(string requestString) =>
            await _requestHelper.GetResponseAsync<List<AnimalType>>(method: HttpMethod.Get,
                                                                    requestUri: requestString);        

        public async Task<List<Animal>> GetAnimalsByAnimalTypesIds(string requestString) =>
            await _requestHelper.GetResponseAsync<List<Animal>>(method: HttpMethod.Get,
                                                                requestUri: requestString);

        public List<ObservedAnimal> GetObservedAnimals(List<Animal> animals, List<AnimalType> animalTypes) =>
            (from animal in animals
             join animalType in animalTypes
             on animal.AnimalTypeId equals animalType.Id
             select new ObservedAnimal
             {
                 Id = animal.Id,
                 Name = animal.Name,
                 AnimalType = animalType.Description
             }).ToList();

        public async Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync(string requestString)
        {
            //string requestString = $"{_animalsApi}/animal/getallanimaltypes";
            return await _requestHelper.GetResponseAsync<List<AnimalType>>(method: HttpMethod.Get,
                                                                     requestUri: requestString);
        }

        public async Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities)
        {
            foreach (Speciality speciality in newSpecialities)
            {
                await _dBContext.Database.ExecuteSqlInterpolatedAsync(
                    $"INSERT INTO Specialities (zookeeperid, animaltypeid) VALUES ({speciality.ZookeeperId}, {speciality.AnimalTypeId}) ON CONFLICT DO NOTHING");
            }
        }

        public async Task DeleteSpecialityAsync(int zookeeperId, int animalTypeId)
        {
            await _dBContext.Specialities.Where(s => s.ZookeeperId == zookeeperId && 
                                                s.AnimalTypeId == animalTypeId).ExecuteDeleteAsync();
            _dBContext.SaveChanges();

        }

        public async Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id)
        {
            return await _dBContext.Jobs.Where(j => j.ZookeeperId == id && 
                                                    j.FinishTime == null)
                                        .OrderBy(j => j.StartTime).ToListAsync();
        }

        public async Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom)
        { 
            return await _dBContext.Jobs.Where(j => j.ZookeeperId == id &&
                                               j.StartTime >= dateTimeFrom)
                                        .OrderBy(j => j.StartTime).ToListAsync();
        }

        public async Task<List<Job>> GetAllJobsOfZookeeperAsync(int id)
        {
            return await _dBContext.Jobs.Where(j => j.ZookeeperId == id)
                                        .OrderBy(j => j.StartTime).ToListAsync();
        }

        public async Task AddJobAsync(int id, Job job)
        {
            if (id == job.ZookeeperId && job.StartTime >= DateTime.UtcNow && job.FinishTime == null)
            {
                await _dBContext.Jobs.AddAsync(job);
                await _dBContext.SaveChangesAsync();
            }
        }

        public async Task DeleteJobAsync(int id, int jobId)
        {
            {
                var job = _dBContext.Jobs.Where(j => j.ZookeeperId == id && j.Id == jobId)
                                         .FirstOrDefault();
                if (job == null)
                    return;
                _dBContext.Jobs.Remove(job);
                await _dBContext.SaveChangesAsync();
            }
        }
        
        public async Task UpdateJobByZookeeperAsync(int id, Job job)
        {
            if(id == job.ZookeeperId)
            {
                _dBContext.Entry(job).Property(x => x.Description).IsModified = true;
                /*_dBContext.Entry(job).Property(x => x.Id). IsModified = false;
                _dBContext.Entry(job).Property(x => x.ZookeeperId).IsModified = false;
                _dBContext.Entry(job).Property(x => x.StartTime).IsModified = false;
                _dBContext.Entry(job).Property(x => x.FinishTime).IsModified = false;*/

                //_dBContext.Jobs.Update(job);
                await _dBContext.SaveChangesAsync();
            }
        }

        public async Task FinishJobAsync(int id, Job job)
        {            
            if (id == job.ZookeeperId && job.FinishTime == null)
            {
                job.FinishTime = DateTime.UtcNow;
                _dBContext.Entry(job).Property(x => x.FinishTime).IsModified = true;
                await _dBContext.SaveChangesAsync();
            }
        }
    }
}
