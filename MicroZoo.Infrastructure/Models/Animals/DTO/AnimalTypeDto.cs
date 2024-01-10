
namespace MicroZoo.Infrastructure.Models.Animals.Dto
{
    public class AnimalTypeDto
    {
        public string Description { get; set; }

        public static AnimalType DtoToAnimalType(AnimalTypeDto animalTypeDto)
        {
            return new AnimalType() 
            { 
                Description = animalTypeDto.Description 
            };
        }
    }
}
