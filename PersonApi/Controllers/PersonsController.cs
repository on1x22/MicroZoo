﻿using Microsoft.AspNetCore.Mvc;
using MicroZoo.AuthService.Policies;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.Models.Persons.Dto;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Controllers
{
    /// <summary>
    /// Controller for handling persons requests
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initialize a new instance of <see cref="PersonsController"/> class 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="configuration"></param>
        /// <param name="receivingService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="receiver"></param>
        /// <param name="connectionService"></param>
        public PersonsController(IServiceProvider provider, IConfiguration configuration,
            IPersonsRequestReceivingService receivingService, 
            IAuthorizationService authorizationService,
            IResponsesReceiverFromRabbitMq receiver,
            IConnectionService connectionService)
        {
            _provider = provider;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
            _receiver = receiver;
            _connectionService = connectionService;
        }

        /// <summary>
        /// Get info about selected person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Person info</returns>
        [HttpGet("{personId}")]
        [PolicyValidation(Policy = "PersonsApi.Read")]
        public async Task<IActionResult> GetPerson(int personId)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                httpRequest: HttpContext.Request,
                type: typeof(PersonsController),
                methodName: nameof(GetPerson),
                IdentityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed) 
                return accessResult.Result;

            var response = await _receivingService.GetPersonAsync(personId);

            return response.Person != null
                ? Ok(response.Person)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="personDto"></param>
        /// <returns>Created person</returns>
        [HttpPost]
        [PolicyValidation(Policy = "PersonsApi.Create")]
        public async Task<IActionResult> AddPerson([FromBody] PersonDto personDto)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                httpRequest: HttpContext.Request,
                type: typeof(PersonsController),
                methodName: nameof(AddPerson),
                IdentityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;

            var response = await _receivingService.AddPersonAsync(personDto);

            return response.Person != null
                ? Ok(response.Person)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Change info about selected person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="personDto"></param>
        /// <returns>Changed info about selected person</returns>
        [HttpPut("{personId}")]
        [PolicyValidation(Policy = "PersonsApi.Update")]
        public async Task<IActionResult> UpdatePerson(int personId, [FromBody] PersonDto personDto)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                httpRequest: HttpContext.Request,
                type: typeof(PersonsController),
                methodName: nameof(UpdatePerson),
                IdentityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;

            var response = await _receivingService.UpdatePersonAsync(personId, personDto); 

            return response.Person != null
                ? Ok(response.Person)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Delete selected person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Deleted person</returns>
        [HttpDelete("{personId}")]
        [PolicyValidation(Policy = "PersonsApi.Delete")]
        public async Task<IActionResult> DeletePerson(int personId)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                httpRequest: HttpContext.Request,
                type: typeof(PersonsController),
                methodName: nameof(DeletePerson),
                IdentityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;
                       
            var response = await _receivingService.DeletePersonAsync(personId);
            
            return response.Person != null
                ? Ok(response.Person)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get information about directly subordinate personnel
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>List of subordinate personnel</returns>
        [HttpGet("{personId}/subordinatePersonnel")]
        [PolicyValidation(Policy = "PersonsApi.Read")]
        public async Task<IActionResult> GetSubordinatePersonnel(int personId)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                httpRequest: HttpContext.Request,
                type: typeof(PersonsController),
                methodName: nameof(GetSubordinatePersonnel),
                IdentityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;

            var response = await _receivingService.GetSubordinatePersonnelAsync(personId);
            
            return response.Persons != null
            ? Ok(response.Persons)
            : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Change manager Id for all directly subordinate personnel
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="newId"></param>
        /// <returns>Subordinate personnel with changer manager Id</returns>
        [HttpPut("Manager/{currentId}/{newId}")]
        [PolicyValidation(Policy = "PersonsApi.Create")]
        public async Task<IActionResult> ChangeManagerForSubordinatePersonnel(int currentId,
                                                                              int newId)
        {
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                httpRequest: HttpContext.Request,
                type: typeof(PersonsController),
                methodName: nameof(ChangeManagerForSubordinatePersonnel),
                IdentityApiUrl: _connectionService.IdentityApiUrl);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;

            var response = await _receivingService.ChangeManagerForSubordinatePersonnel(currentId,
                newId);
            
            return response.Persons != null
            ? Ok(response.Persons)
            : BadRequest(response.ErrorMessage);
        }
    }
}
