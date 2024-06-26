﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MicroZoo.Infrastructure.Models.Jobs
{
    [Table("jobs")]
    public class Job
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("zookeeperid")]
        public int ZookeeperId { get; set; }

        [Column("description"), MaxLength(150)]
        public string Description { get; set; }

        [Column("starttime"), NotNull]
        public DateTime StartTime { get; set; }

        [Column("deadlinetime"), NotNull]
        public DateTime DeadlineTime { get; set; }

        [Column("finishtime")]
        public DateTime? FinishTime { get; set; }

        [Column("report"), MaxLength(250)]
        public string? Report {  get; set; }

        [Column("priority")]
        public int Priority {  get; set; }
        
        [Column("createdat"), NotNull]
        public DateTime CreatedAt { get; set; }
    }
}
