﻿using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.DBContext;

namespace MicroZoo.ZookeepersApi.Repository
{
    public class JobsRepository : IJobsRepository
    {
        private readonly ZookeeperDBContext _dBContext;
        
        public JobsRepository(ZookeeperDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Job>> GetAllJobsOfZookeeperAsync(int zookeeperId) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == zookeeperId)
                                        .OrderBy(j => j.StartTime).ToListAsync();

        public async Task<List<Job>> GetCurrentJobsOfZookeeperAsync(int zookeeperId) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == zookeeperId && j.FinishTime == null)
                                 .OrderBy(j => j.StartTime).ToListAsync();

        public async Task<List<Job>> GetAllJobsForTimeRangeAsync(DateTime startDateTime, 
            DateTime finishDateTime) =>
            await _dBContext.Jobs.Where(j => (j.StartTime >= startDateTime && 
            j.StartTime < finishDateTime) &&
            (j.FinishTime <= finishDateTime || j.FinishTime == null)).ToListAsync();

        public async Task<List<Job>> GetZookeeperJobsForTimeRangeAsync(int zookeeperId,
            DateTime startDateTime, DateTime finishDateTime) =>
            await _dBContext.Jobs.Where(j => j.ZookeeperId == zookeeperId &&
            (j.StartTime >= startDateTime && j.StartTime < finishDateTime) && 
            (j.FinishTime <= finishDateTime || j.FinishTime == null)).ToListAsync();

        public async Task<Job> GetJobAsync(int jobId) =>
            await _dBContext.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
        

        public async Task<Job> AddJobAsync(JobDto jobDto)
        {
            var job = jobDto.ToJob();

            await _dBContext.Jobs.AddAsync(job);
            await SaveChangesAsync();

            return job;
        }

        public async Task<Job> UpdateJobAsync(int jobId, JobWithoutStartTimeDto jobDto)
        {
            var updatedJob = await _dBContext.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
            if (updatedJob != null)
            {
                updatedJob.ZookeeperId = jobDto.ZookeeperId;
                updatedJob.Description = jobDto.Description;
            }
            await SaveChangesAsync();

            return updatedJob;
        }

        public async Task<Job> FinishJobAsync(int jobId)
        {
            var finishedJob = await _dBContext.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
            if (finishedJob != null)
                finishedJob.FinishTime = DateTime.UtcNow;
            
            await SaveChangesAsync();

            return finishedJob;
        }

        private async Task SaveChangesAsync() =>
            await _dBContext.SaveChangesAsync();

        
    }
}
