﻿using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.Models.Animals.Dto;
using MicroZoo.JwtConfiguration;
using MicroZoo.Infrastructure.MassTransit;

namespace MicroZoo.AnimalsApi.Controllers
{
    /// <summary>
    /// Controller for handling animals requests
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;
        private readonly IRabbitMqResponseErrorsHandler _errorsHandler;

        /// <summary>
        /// Initialize a new instance of <see cref="AnimalsController"/> class
        /// </summary>        
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="connectionService"></param>
        /// <param name="errorsHandler"></param>
        public AnimalsController(IAnimalsRequestReceivingService receivingService,
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
        /// Get all animals
        /// </summary>
        /// <returns>List of animals</returns>
        [HttpGet]
        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task<IActionResult> GetAllAnimals()
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalsController),
                methodName: nameof(GetAllAnimals),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.GetAllAnimalsAsync();
            
            return response.Animals != null
                ? Ok(response.Animals)
                : NoContent();
        }

        /// <summary>
        /// Get info about selected animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Animal info</returns>
        [HttpGet("{animalId}")]
        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task<IActionResult> GetAnimal(int animalId)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalsController),
                methodName: nameof(GetAnimal),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.GetAnimalAsync(animalId);

            return response.Animal != null
                ? Ok(response.Animal)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Create new animal
        /// </summary>
        /// <param name="animalDto"></param>
        /// <returns>Created animal</returns>
        [HttpPost]
        [PolicyValidation(Policy = "AnimalsApi.Create")]
        public async Task<IActionResult> AddAnimal([FromBody] AnimalDto animalDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalsController),
                methodName: nameof(AddAnimal),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.AddAnimalAsync(animalDto);

            return response.Animal != null
                ? Ok(response.Animal)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Change data about selected animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="animalDto"></param>
        /// <returns>Changed info about selected animal</returns>
        [HttpPut("{animalId}")]
        [PolicyValidation(Policy = "AnimalsApi.Update")]
        public async Task<IActionResult> UpdateAnimal(int animalId, [FromBody] AnimalDto animalDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalsController),
                methodName: nameof(UpdateAnimal),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.UpdateAnimalAsync(animalId, animalDto);

            return response.Animal != null
                ? Ok(response.Animal)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Delete selected animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Deleted animal</returns>
        [HttpDelete("{animalId}")]
        [PolicyValidation(Policy = "AnimalsApi.Delete")]
        public async Task<IActionResult> DeleteAnimal(int animalId)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalsController),
                methodName: nameof(DeleteAnimal),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.DeleteAnimalAsync(animalId);
            
            return response.Animal != null
                ? Ok(response.Animal)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get animals by selected animal types Ids
        /// </summary>
        /// <param name="animalTypesIds)"></param>
        /// <returns>List of animals by selected animal types</returns>
        [HttpGet("byTypes")]
        [PolicyValidation(Policy = "AnimalsApi.Read")]
        public async Task<IActionResult> GetAnimalsByTypes([FromQuery] int[] animalTypesIds)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(AnimalsController),
                methodName: nameof(GetAnimalsByTypes),
                _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return _errorsHandler.GetActionResult(accessResult);

            var response = await _receivingService.GetAnimalsByTypesAsync(animalTypesIds);
            
            return response.Animals != null
            ? Ok(response.Animals)
            : BadRequest(response.ErrorMessage);
        }
    }
}
