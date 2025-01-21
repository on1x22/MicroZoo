using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.Models.Animals.Dto;
using MicroZoo.JwtConfiguration;

namespace MicroZoo.AnimalsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnimalTypesController : ControllerBase
    {
        private readonly IAnimalTypesRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;
        private readonly IRabbitMqResponseErrorsHandler _errorsHandler;

        public AnimalTypesController(IAnimalTypesRequestReceivingService receivingService,
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
        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task<IActionResult> GetAllAnimalTypes()
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalTypesController),
                methodName: nameof(GetAllAnimalTypes),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)   
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.GetAllAnimalTypesAsync();

            return response.AnimalTypes != null
                ? Ok(response.AnimalTypes)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get info about selected animal type
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Animal type info</returns>
        [HttpGet("{animalTypeId}")]
        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task<IActionResult> GetAnimalType(int animalTypeId)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalTypesController),
                methodName: nameof(GetAnimalType),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.GetAnimalTypeAsync(animalTypeId);

            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Create new animal type
        /// </summary>
        /// <param name="animalTypeDto"></param>
        /// <returns>Created animal type</returns>
        [HttpPost]
        [PolicyValidation(Policy = "AnimalsApi.Create")]
        public async Task<IActionResult> AddAnimalType([FromBody] AnimalTypeDto animalTypeDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalTypesController),
                methodName: nameof(AddAnimalType),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.AddAnimalTypeAsync(animalTypeDto);
            
            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Change data about selected animal type
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <param name="animalTypeDto"></param>
        /// <returns>Changed info about selected animal type</returns>
        [HttpPut("{animalTypeId}")]
        [PolicyValidation(Policy = "AnimalsApi.Update")]
        public async Task<IActionResult> UpdateAnimalType(int animalTypeId, 
            [FromBody] AnimalTypeDto animalTypeDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalTypesController),
                methodName: nameof(UpdateAnimalType),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.UpdateAnimalTypeAsync(animalTypeId, animalTypeDto);
            
            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Delete selected animal type
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Deleted animal type</returns>
        [HttpDelete("{animalTypeId}")]
        [PolicyValidation(Policy = "AnimalsApi.Delete")]
        public async Task<IActionResult> DeleteAnimalType(int animalTypeId)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalTypesController),
                methodName: nameof(DeleteAnimalType),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.DeleteAnimalTypeAsync(animalTypeId, accessToken);
            
            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get animal types by selected Ids
        /// </summary>
        /// <param name="animalTypesIds)"></param>
        /// <returns>List of selected animal types</returns>
        [HttpGet("byIds")]
        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task<IActionResult> GetAnimalTypesByIds([FromQuery] int[] animalTypesIds)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalTypesController),
                methodName: nameof(GetAnimalTypesByIds),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.GetAnimalTypesByIdsAsync(animalTypesIds);
            
            return response.AnimalTypes != null
            ? Ok(response.AnimalTypes)
            : BadRequest(response.ErrorMessage); 
        }
    }
}
