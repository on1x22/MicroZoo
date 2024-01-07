
namespace MicroZoo.Infrastructure.Models.Animals.Dto
{
    public class AnimalDto
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public int AnimalTypeId { get; set; }

        public static Animal DtoToAnimal(AnimalDto animalDto)
        {
            if (animalDto == null)
                throw new ArgumentNullException("Invalid data entered to create animal");

            return new Animal()
            {
                Name = animalDto!.Name,
                Link = animalDto!.Link,
                AnimalTypeId = animalDto.AnimalTypeId
            };
        }
    }
}
