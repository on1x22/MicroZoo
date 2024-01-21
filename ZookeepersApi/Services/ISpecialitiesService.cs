using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface ISpecialitiesService
    {
        Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId, 
            SpecialityDto specialityDto);
    }
}
