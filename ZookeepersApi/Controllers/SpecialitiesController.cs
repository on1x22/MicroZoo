using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.JwtConfiguration;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        private readonly ISpecialitiesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;

        public SpecialitiesController(ISpecialitiesRequestReceivingService receivingService,
            IAuthorizationService authorizationService, IConnectionService connectionService)
        {
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
        }

        /// <summary>
        /// Get all animal types
        /// </summary>
        /// <returns>List of animal types</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSpecialities()
        {
            var response = await _receivingService.GetAllSpecialitiesAsync();

            return response.AnimalTypes != null
                ? Ok(response.AnimalTypes)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Check that there is an association between specialty and any zookeepers
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>True or false</returns>
        [HttpGet("animalTypes/{animalTypeId}")]
        public async Task<IActionResult> CheckZokeepersWithSpecialityAreExist(int animalTypeId)
        {
            var response = await _receivingService.CheckZokeepersWithSpecialityAreExistAsync(CheckType.AnimalType, 
                animalTypeId);

            return Ok(response.IsThereZookeeperWithThisSpeciality);
        }
         
        /// <summary>
        /// Check that zookeeper is associated with any specialty
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>True or false</returns>
        [HttpGet("zookeepers/{personId}")]
        public async Task<IActionResult> CheckZookeeperIsExist(int personId)
        {
            var response = await _receivingService.CheckZookeeperIsExistAsync(CheckType.Person, personId);

            return Ok(response.IsThereZookeeperWithThisSpeciality);
        }

        /// <summary>
        /// Add new speciality
        /// </summary>
        /// <param name="specialityDto"></param>
        /// <returns>Speciality</returns>
        [HttpPost]
        public async Task<IActionResult> AddSpeciality([FromBody] SpecialityDto specialityDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(SpecialitiesController),
                methodName: nameof(AddSpeciality),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;

            var response = await _receivingService.AddSpecialityAsync(specialityDto, accessToken);

            return response.Speciality != null
                ? Ok(response.Speciality)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Change relation between zookeeper and speciality
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="specialityDto"></param>
        /// <returns>Speciality</returns>
        [HttpPut("{relationId}")]
        public async Task<IActionResult> ChangeRelationBetweenZookeeperAndSpeciality(int relationId,
            [FromBody] SpecialityDto specialityDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(SpecialitiesController),
                methodName: nameof(ChangeRelationBetweenZookeeperAndSpeciality),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;

            var response = await _receivingService.ChangeRelationBetweenZookeeperAndSpecialityAsync(
                relationId, specialityDto, accessToken);

            return response.Speciality != null
                ? Ok(response.Speciality)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Delete the specialty specified in the request body
        /// </summary>
        /// <param name="specialityDto"></param>
        /// <returns>List of zookeeper specialities</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSpeciality([FromBody] SpecialityDto specialityDto)
        {
            var response = await _receivingService.DeleteSpecialityAsync(specialityDto);

            return response.AnimalTypes != null
            ? Ok(response.AnimalTypes)
            : BadRequest(response.ErrorMessage);
        }
    }
}
