﻿using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface ISpecialitiesService
    {        
        /// <summary>
        /// Returns true, if one o more zokeepers with speciality exist in database
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="objectId"></param>
        /// <returns>True of false</returns>
        Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZokeepersWithSpecialityAreExistAsync(
            CheckType checkType, int objectId);
        
        Task<GetSpecialityResponse> AddSpecialityAsync(SpecialityDto specialityDto);

        Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId,
            SpecialityDto specialityDto);

        Task<GetSpecialitiesResponse> DeleteSpecialityAsync(SpecialityDto specialityDto);
    }
}
