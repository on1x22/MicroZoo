
namespace MicroZoo.Infrastructure.Models.Jobs.Dto
{
    public class JobWithoutStartTimeDto
    {
        public int ZookeeperId { get; set; }
        public string Description { get; set; }
        public DateTime DeadlineTime { get; set; }
        public int Priority { get; set; }
    }
}
