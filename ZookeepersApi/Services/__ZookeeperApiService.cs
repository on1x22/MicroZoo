﻿using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;

namespace MicroZoo.ZookeepersApi.Services
{
    public class __ZookeeperApiService : __IZookeeperApiService
    {
        private readonly __IZookeeperRepository _repository;
        private readonly string _personsApi;
        private readonly string _animalsApi;

        public __ZookeeperApiService(__IZookeeperRepository repository) 
        {
            _repository = repository;
            _personsApi = Environment.GetEnvironmentVariable("PERSONS_API");
            _animalsApi = Environment.GetEnvironmentVariable("ANIMALS_API");
        }

        public async Task<ZookeeperInfo> GetZookepeerInfoAsync(int id)
        {
            ZookeeperInfo zookeeperInfo = new ZookeeperInfo();

            zookeeperInfo.Adout = await GetPersonByIdFromPersonsApiAsync(id);
            if (zookeeperInfo.Adout == null)
                return default;

            zookeeperInfo.Jobs = await _repository.GetJobsById(id);

            var idOfSpecialities = await _repository.GetSpecialitiesIdByPersonId(id);            

            if (idOfSpecialities != null && idOfSpecialities.Count > 0)
            {
                string parameters = "animalTypeIds=" + String.Join("&animalTypeIds=", idOfSpecialities);
                string requestString = $"{_animalsApi}/animal/getanimaltypesbyid?" + parameters;

                zookeeperInfo.Specialities = await _repository.GetAnimalTypesByIds(requestString);

                if (zookeeperInfo.Specialities == null || zookeeperInfo.Specialities.Count == 0)
                    return zookeeperInfo;

                requestString = $"{_animalsApi}/animal/getanimalsbytypes2?" + parameters;
                var animals = await _repository.GetAnimalsByAnimalTypesIds(requestString);

                zookeeperInfo.ObservedAnimals = _repository.GetObservedAnimals(animals, zookeeperInfo.Specialities);
            }
            return zookeeperInfo;
        }

        public async Task<Person> GetPersonByIdFromPersonsApiAsync(int id)
        {
            string requestString = $"{_personsApi}/person/{id}";

            return await _repository.GetPersonByIdFromPersonsApiAsync(requestString);
        }
        
        
            
        

        







        [Obsolete("Old solution")]
        public async Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync()
        {
            string requestString = $"{_animalsApi}/animal/getallanimaltypes";

            return await _repository.GetAllAnimalTypesFromAnimalsApiAsync(requestString);
        }

        [Obsolete("Old solution")]
        public async Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities) =>
            await _repository.ChangeSpecialitiesAsync(newSpecialities);

        [Obsolete("Old solution")]
        public async Task DeleteSpecialityAsync(int zookeeperId, int animalTypeId) =>
            await _repository.DeleteSpecialityAsync(zookeeperId, animalTypeId);

        [Obsolete("Old solution")]
        public async Task<List<Job>> GetAllJobsOfZookeeperAsync(int id) =>
            await _repository.GetAllJobsOfZookeeperAsync(id);

        [Obsolete("Old solution")]
        public async Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id) =>
            await _repository.GetCurrentJobsOfZookeeperAsync(id);

        [Obsolete("Old solution")]
        public async Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom) =>
            await _repository.GetJobsOfZookeeperFromAsync(id, dateTimeFrom);

        [Obsolete("Old solution")]
        public async Task AddJobAsync(int id, Job job) =>
            await _repository.AddJobAsync(id, job);

        [Obsolete("Old solution")]
        public async Task UpdateJobByZookeeperAsync(int id, Job job) =>
            await _repository.UpdateJobByZookeeperAsync(id, job);

        [Obsolete("Old solution")]
        public async Task FinishJobAsync(int id, Job job) =>
            await _repository.FinishJobAsync(id, job);

        [Obsolete("Old solution")]
        public async Task DeleteJobAsync(int id, int jobId) =>
            await _repository.DeleteJobAsync(id, jobId);
    }
}
