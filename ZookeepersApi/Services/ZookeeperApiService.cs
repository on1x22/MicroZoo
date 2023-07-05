using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;

namespace MicroZoo.ZookeepersApi.Services
{
    public class ZookeeperApiService : IZookeeperApiService
    {
        private readonly IZookeeperRepository _repository;
        //private readonly RequestHelper _requestHelper;
        private readonly string _personsApi;
        private readonly string _animalsApi;

        public ZookeeperApiService(IZookeeperRepository repository/*, RequestHelper requestHelper*/) 
        {
            _repository = repository;
            //_requestHelper = requestHelper;
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

                if (zookeeperInfo.Specialities.Count == 0)
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

        public async Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync()
        {
            string requestString = $"{_animalsApi}/animal/getallanimaltypes";

            return await _repository.GetAllAnimalTypesFromAnimalsApiAsync(requestString);
        }

        public async Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities) =>        
            await _repository.ChangeSpecialitiesAsync(newSpecialities);
        
        public async Task DeleteSpecialityAsync(int zookeeperId, int animalTypeId) =>        
            await _repository.DeleteSpecialityAsync(zookeeperId, animalTypeId);        

        public async Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id) =>        
            await _repository.GetCurrentJobsOfZookeeperAsync(id);

        public async Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom) =>        
            await _repository.GetJobsOfZookeeperFromAsync(id, dateTimeFrom);
        
        public async Task<List<Job>> GetAllJobsOfZookeeperAsync(int id) =>        
            await _repository.GetAllJobsOfZookeeperAsync(id);
        
        public async Task AddJobAsync(int id, Job job) =>        
            await _repository.AddJobAsync(id, job);

        public async Task DeleteJobAsync(int id, int jobId) =>        
            await _repository.DeleteJobAsync(id, jobId);

        public async Task UpdateJobByZookeeperAsync(int id, Job job) =>
            await _repository.UpdateJobByZookeeperAsync(id, job);

        public async Task FinishJobAsync(int id, Job job) =>
            await _repository.FinishJobAsync(id, job);
        
    }
}
