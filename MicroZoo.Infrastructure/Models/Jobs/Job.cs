using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MicroZoo.Infrastructure.Models.Jobs
{
    /// <summary>
    /// Provides information about job
    /// </summary>
    [Table("jobs")]
    public class Job
    {
        /// <summary>
        /// Job id
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Id of zookeeper that doing this job
        /// </summary>
        [Column("zookeeperid")]
        public int ZookeeperId { get; set; }

        /// <summary>
        /// Job description
        /// </summary>
        [Column("description"), MaxLength(150)]
        public string? Description { get; set; }

        /// <summary>
        /// Start time of job
        /// </summary>
        [Column("starttime"), NotNull]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Time that job must be done
        /// </summary>
        [Column("deadlinetime"), NotNull]
        public DateTime DeadlineTime { get; set; }

        /// <summary>
        /// Actual time of completion of work
        /// </summary>
        [Column("finishtime")]
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// Report of job completion
        /// </summary>
        [Column("report"), MaxLength(250)]
        public string? Report {  get; set; }

        /// <summary>
        /// Job priority
        /// </summary>
        [Column("priority")]
        public int Priority {  get; set; }
        
        /// <summary>
        /// Time of job creation
        /// </summary>
        [Column("createdat"), NotNull]
        public DateTime CreatedAt { get; set; }
    }
}
