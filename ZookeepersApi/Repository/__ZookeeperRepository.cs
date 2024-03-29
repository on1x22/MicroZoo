﻿using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.ZookeepersApi.Repository
{
    public class __ZookeeperRepository : __IZookeeperRepository
    {
        private readonly ZookeeperDBContext _dBContext;
        private readonly RequestHelper _requestHelper;
        private readonly string _personsApi;
        private readonly string _animalsApi;

        public __ZookeeperRepository(ZookeeperDBContext dBContext, RequestHelper requestHelper)
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

        

        public async Task<Person> GetPersonByIdFromPersonsApiAsync(string requestString) =>        
            await _requestHelper.GetResponseAsync<Person>(method: HttpMethod.Get,
                                                                 requestUri: requestString);
        
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
                
        
                   
        
        
        








        [Obsolete("Old solution")]
        public async Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync(string requestString) =>
            await _requestHelper.GetResponseAsync<List<AnimalType>>(method: HttpMethod.Get,
                                                                     requestUri: requestString);

        [Obsolete("Old solution")]
        public async Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities)
        {
            foreach (Speciality speciality in newSpecialities)
            {
                await _dBContext.Database.ExecuteSqlInterpolatedAsync(
                    $"INSERT INTO Specialities (zookeeperid, animaltypeid) VALUES ({speciality.ZookeeperId}, {speciality.AnimalTypeId}) ON CONFLICT DO NOTHING");
            }
        }

        [Obsolete("Old solution")]
        public async Task DeleteSpecialityAsync(int zookeeperId, int animalTypeId)
        {
            await _dBContext.Specialities.Where(s => s.ZookeeperId == zookeeperId &&
                                                s.AnimalTypeId == animalTypeId).ExecuteDeleteAsync();
            _dBContext.SaveChanges();
        }

        [Obsolete("Old solution")]
        public async Task<List<Job>> GetAllJobsOfZookeeperAsync(int id) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == id)
                                        .OrderBy(j => j.StartTime).ToListAsync();

        [Obsolete("Old solution")]
        public async Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == id &&
                                                    j.FinishTime == null)
                                        .OrderBy(j => j.StartTime).ToListAsync();

        [Obsolete("Old solution")]
        public async Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == id &&
                                               j.StartTime >= dateTimeFrom)
                                        .OrderBy(j => j.StartTime).ToListAsync();

        [Obsolete("Old solution")]
        public async Task AddJobAsync(int id, Job job)
        {
            if (id == job.ZookeeperId && job.StartTime >= DateTime.UtcNow && job.FinishTime == null)
            {
                await _dBContext.Jobs.AddAsync(job);
                await _dBContext.SaveChangesAsync();
            }
        }

        [Obsolete("Old solution")]
        public async Task UpdateJobByZookeeperAsync(int id, Job job)
        {
            if (id == job.ZookeeperId)
            {
                _dBContext.Entry(job).Property(x => x.Description).IsModified = true;
                
                await _dBContext.SaveChangesAsync();
            }
        }

        [Obsolete("Old solution")]
        public async Task FinishJobAsync(int id, Job job)
        {
            if (id == job.ZookeeperId && job.FinishTime == null)
            {
                job.FinishTime = DateTime.UtcNow;
                _dBContext.Entry(job).Property(x => x.FinishTime).IsModified = true;
                await _dBContext.SaveChangesAsync();
            }
        }

        [Obsolete("Old solution")]
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
    }
}
