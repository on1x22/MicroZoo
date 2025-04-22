
namespace MicroZoo.Infrastructure.Models.Animals.Dto
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about an animal
    /// </summary>
    public class AnimalDto
    {
        /// <summary>
        /// Name of animal
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Link to a site with information about the animal
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// Id of type of the animal
        /// </summary>
        public int AnimalTypeId { get; set; }

        /// <summary>
        /// Convert instane of <see cref="AnimalDto"/> class to the instance 
        /// of the <see cref="Animal"/> class
        /// </summary>
        /// <returns></returns>
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
