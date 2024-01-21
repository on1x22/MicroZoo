using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.ZookeepersApi.Models
{
    public class ZookeeperInfo
    {
        public Person Adout { get; set; }
        public List<Job> Jobs { get; set; }
        public  List<AnimalType> Specialities { get; set; }
        public List<ObservedAnimal> ObservedAnimals { get; set; }
    }
}
