
namespace MicroZoo.Infrastructure.Models.Jobs.Dto
{
    /// <summary>
    /// DTO obtained from controllers and other microservices and 
    /// provides information about a job
    /// </summary>
    public class JobDto : JobWithoutStartTimeDto
    {
        /// <summary>
        /// Start time of job
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Convert instane of <see cref="JobDto"/> class to the instance 
        /// of the <see cref="Job"/> class
        /// </summary>
        /// <returns></returns>
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
