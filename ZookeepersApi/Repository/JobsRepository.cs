using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.DBContext;
using MicroZoo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.Generals;

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

        public async Task<List<Job>> GetAllJobsForDateTimeRangeAsync(DateTimeRange dateTimeRange, 
            OrderingOptions orderingOptions, PageOptions pageOptions) =>            
            await _dBContext.Jobs.Where(j => (j.StartTime >= dateTimeRange.StartDateTime &&
            j.StartTime < dateTimeRange.FinishDateTime) &&
            (j.FinishTime <= dateTimeRange.FinishDateTime || j.FinishTime == null))
            .OrderBy(propertyName: orderingOptions.PropertyName, 
                descending: orderingOptions.OrderDescending)
            .Skip((pageOptions.PageNumber - 1) * pageOptions.ItemsOnPage)
            .Take(pageOptions.ItemsOnPage)
            .ToListAsync();

        public async Task<List<Job>> GetZookeeperJobsForDateTimeRangeAsync(int zookeeperId,
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions) =>            
            await _dBContext.Jobs.Where(j => j.ZookeeperId == zookeeperId &&
            (j.StartTime >= dateTimeRange.StartDateTime && j.StartTime < dateTimeRange.FinishDateTime) &&
            (j.FinishTime <= dateTimeRange.FinishDateTime || j.FinishTime == null))
            .OrderBy(propertyName: orderingOptions.PropertyName,
                descending: orderingOptions.OrderDescending)
            .Skip((pageOptions.PageNumber - 1) * pageOptions.ItemsOnPage)
            .Take(pageOptions.ItemsOnPage)
            .ToListAsync();

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
                updatedJob.DeadlineTime = jobDto.DeadlineTime;
                updatedJob.Priority = jobDto.Priority;
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
