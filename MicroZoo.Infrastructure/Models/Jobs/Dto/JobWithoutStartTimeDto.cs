
namespace MicroZoo.Infrastructure.Models.Jobs.Dto
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about a job but without start time
    /// </summary>
    public class JobWithoutStartTimeDto
    {
        /// <summary>
        /// Id of zookeeper that doing this job
        /// </summary>
        public int ZookeeperId { get; set; }

        /// <summary>
        /// Job description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Time that job must be done
        /// </summary>
        public DateTime DeadlineTime { get; set; }

        /// <summary>
        /// Job priority
        /// </summary>
        public int Priority { get; set; }
    }
}
