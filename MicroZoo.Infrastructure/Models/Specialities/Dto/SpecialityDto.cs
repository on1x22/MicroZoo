
namespace MicroZoo.Infrastructure.Models.Specialities.Dto
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about speciality
    /// </summary>
    public class SpecialityDto
    {
        /// <summary>
        /// Zookeeper's id
        /// </summary>
        public int ZookeeperId { get; set; }

        /// <summary>
        /// Animal type id
        /// </summary>
        public int AnimalTypeId { get; set; }

        /// <summary>
        /// Returns instance of <see cref="Speciality"/> with same data
        /// </summary>
        /// <returns></returns>
        public Speciality ToSpeciality() =>
            new Speciality()
            {
                ZookeeperId = ZookeeperId,
                AnimalTypeId = AnimalTypeId
            };
        
    }
}
