﻿using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.ZookeepersApi.Repository
{
    public interface __IZookeeperRepository
    {
        // TODO: remove not actual methods
        Task<Zookeeper> GetByNameAsync(string name);
        Task<Person> GetPersonByIdFromPersonsApiAsync(string requestString);
        Task<List<Zookeeper>> GetAllAsync();
        Task<List<Zookeeper>> GetBySpecialityAsync(string speciality);

        

        Task<List<Job>> GetJobsById(int id);
        Task<List<int>> GetSpecialitiesIdByPersonId(int id);
        Task<List<AnimalType>> GetAnimalTypesByIds(string requestString);
        Task<List<Animal>> GetAnimalsByAnimalTypesIds(string requestString);
        List<ObservedAnimal> GetObservedAnimals(List<Animal> animals, List<AnimalType> animalTypes);


        
        
        
               
        
        







        [Obsolete("Old solution")]
        Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync(string requestString);

        [Obsolete("Old solution")]
        Task ChangeSpecialitiesAsync(List<Speciality> newSpecialities);


        [Obsolete("Old solution")]
        Task DeleteSpecialityAsync(int zookeeperid, int animaltypeid);



        [Obsolete("Old solution")]
        Task<List<Job>> GetAllJobsOfZookeeperAsync(int id);

        [Obsolete("Old solution")]
        Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int id);

        [Obsolete("Old solution")]
        Task<List<Job>> GetJobsOfZookeeperFromAsync(int id, DateTime dateTimeFrom);

        [Obsolete("Old solution")]
        Task AddJobAsync(int id, Job job);

        [Obsolete("Old solution")]
        Task UpdateJobByZookeeperAsync(int id, Job job);

        [Obsolete("Old solution")]
        Task FinishJobAsync(int id, Job job);

        [Obsolete("Old solution")]
        Task DeleteJobAsync(int id, int jobId);
    }
}
