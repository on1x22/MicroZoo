using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.Models.Persons.Dto;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;

namespace MicroZoo.PersonsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/persons-queue");

        public PersonsController(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Get info about selected person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Person info</returns>
        [HttpGet("{personId}")]
        public async Task<IActionResult> GetPerson(int personId)
        {
            var response = await GetResponseFromRabbitTask<GetPersonRequest, 
                GetPersonResponse>(new GetPersonRequest(personId));

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
        public async Task<IActionResult> AddPerson([FromBody] PersonDto personDto)
        {
            var response = await GetResponseFromRabbitTask<AddPersonRequest,
                GetPersonResponse>(new AddPersonRequest(personDto));

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
        public async Task<IActionResult> UpdatePerson(int personId, [FromBody] PersonDto personDto)
        {
            var response = await GetResponseFromRabbitTask<UpdatePersonRequest, 
                GetPersonResponse>(new UpdatePersonRequest(personId, personDto));

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
        public async Task<IActionResult> DeletePerson(int personId)
        {
            var response = await GetResponseFromRabbitTask<DeletePersonRequest, GetPersonResponse>(new DeletePersonRequest(personId));
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
        public async Task<IActionResult> GetSubordinatePersonnel(int personId)
        {
            var response = await GetResponseFromRabbitTask<GetSubordinatePersonnelRequest,
                GetPersonsResponse>(new GetSubordinatePersonnelRequest(personId));
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
        public async Task<IActionResult> ChangeManagerForSubordinatePersonnel(
            int currentId,
            int newId
            )
        {
            var response = await GetResponseFromRabbitTask<ChangeManagerForSubordinatePersonnelRequest,
                GetPersonsResponse>(new ChangeManagerForSubordinatePersonnelRequest(currentId, 
                                                                                    newId));
            return response.Persons != null
            ? Ok(response.Persons)
            : BadRequest(response.ErrorMessage);
        }

        private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(_rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
