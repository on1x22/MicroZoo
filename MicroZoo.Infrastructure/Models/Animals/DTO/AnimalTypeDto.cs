
namespace MicroZoo.Infrastructure.Models.Animals.Dto
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about a type of the animal
    /// </summary>
    public class AnimalTypeDto
    {
        /// <summary>
        /// Animal type description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Convert instane of <see cref="AnimalTypeDto"/> class to the instance 
        /// of the <see cref="AnimalType"/> class
        /// </summary>
        /// <returns></returns>
        public static AnimalType DtoToAnimalType(AnimalTypeDto animalTypeDto)
        {
            return new AnimalType() 
            { 
                Description = animalTypeDto.Description 
            };
        }
    }
}
