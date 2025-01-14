using Microsoft.AspNetCore.Mvc;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.JwtConfiguration;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Controllers
{
    /// <summary>
    /// Controller for handling specialities requests
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        private readonly ISpecialitiesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;
        private readonly IRabbitMqResponseErrorsHandler _errorsHandler;

        /// <summary>
        /// Initialize a new instance of <see cref="SpecialitiesController"/> class
        /// </summary>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
        /// <param name="errorsHandler"></param>
        public SpecialitiesController(ISpecialitiesRequestReceivingService receivingService,
            IAuthorizationService authorizationService, 
            IConnectionService connectionService,
            IRabbitMqResponseErrorsHandler errorsHandler)
        {
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
            _errorsHandler = errorsHandler;
        }

        /// <summary>
        /// Get all animal types
        /// </summary>
        /// <returns>List of animal types</returns>
        [HttpGet]
        [PolicyValidation(Policy = "ZookeepersApi.Read")]
        public async Task<IActionResult> GetAllSpecialities()
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(SpecialitiesController),
                methodName: nameof(GetAllSpecialities),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                //return accessResult.Result;
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.GetAllSpecialitiesAsync(accessToken);

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
        [PolicyValidation(Policy = "ZookeepersApi.Read")]
        public async Task<IActionResult> CheckZokeepersWithSpecialityAreExist(int animalTypeId)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(SpecialitiesController),
                methodName: nameof(CheckZokeepersWithSpecialityAreExist),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                //return accessResult.Result;
                return _errorsHandler.GetActionResult(accessResult);

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
        [PolicyValidation(Policy = "ZookeepersApi.Read")]
        public async Task<IActionResult> CheckZookeeperIsExist(int personId)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(SpecialitiesController),
                methodName: nameof(CheckZookeeperIsExist),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                //return accessResult.Result;
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.CheckZookeeperIsExistAsync(CheckType.Person, personId);

            return Ok(response.IsThereZookeeperWithThisSpeciality);
        }

        /// <summary>
        /// Add new speciality
        /// </summary>
        /// <param name="specialityDto"></param>
        /// <returns>Speciality</returns>
        [HttpPost]
        [PolicyValidation(Policy = "ZookeepersApi.Create")]
        public async Task<IActionResult> AddSpeciality([FromBody] SpecialityDto specialityDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(SpecialitiesController),
                methodName: nameof(AddSpeciality),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                //return accessResult.Result;
                return _errorsHandler.GetActionResult(accessResult);

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
        [PolicyValidation(Policy = "ZookeepersApi.Update")]
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
                //return accessResult.Result;
                return _errorsHandler.GetActionResult(accessResult);

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
        [PolicyValidation(Policy = "ZookeepersApi.Delete")]
        public async Task<IActionResult> DeleteSpeciality([FromBody] SpecialityDto specialityDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(SpecialitiesController),
                methodName: nameof(DeleteSpeciality),
                identityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                //return accessResult.Result;
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.DeleteSpecialityAsync(specialityDto, accessToken);

            return response.AnimalTypes != null
            ? Ok(response.AnimalTypes)
            : BadRequest(response.ErrorMessage);
        }
    }
}
