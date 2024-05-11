
namespace MicroZoo.Infrastructure.Models.Jobs.Dto
{
    public class JobDto
    {
        public int ZookeeperId { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DeadlineTime { get; set; }
        public int Priority { get; set; }

        public Job ToJob() =>
            new Job()
            {
                ZookeeperId = ZookeeperId,
                Description = Description,
                StartTime = StartTime,
                DeadlineTime = DeadlineTime,
                Priority = Priority,
                CreatedAt = DateTime.UtcNow
            };
       
    }
}
