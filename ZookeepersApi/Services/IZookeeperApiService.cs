using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface IZookeeperApiService
    {
        Task<ZookeeperInfo> GetZookepeerInfoAsync(int id);
        Task<Person> GetPersonByIdFromPersonsApiAsync(int id);

        Task<List<AnimalType>> GetAllAnimalTypesFromAnimalsApiAsync();
    }
}
