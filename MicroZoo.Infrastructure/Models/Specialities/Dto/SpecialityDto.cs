
namespace MicroZoo.Infrastructure.Models.Specialities.Dto
{
    public class SpecialityDto
    {
        public int ZookeeperId { get; set; }
        public int AnimalTypeId { get; set; }

        public Speciality ToSpeciality() =>
            new Speciality()
            {
                ZookeeperId = ZookeeperId,
                AnimalTypeId = AnimalTypeId
            };
        
    }
}
