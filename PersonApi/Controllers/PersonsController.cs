using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.Models.Persons.Dto;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.PersonsApi.Services;

namespace MicroZoo.PersonsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly IPersonsRequestReceivingService _receivingService;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/persons-queue");
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public PersonsController(IServiceProvider provider, IConfiguration configuration,
            IPersonsRequestReceivingService receivingService)
        {
            _provider = provider;
            _receivingService = receivingService;
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        /// <summary>
        /// Get info about selected person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Person info</returns>
        [HttpGet("{personId}")]
        public async Task<IActionResult> GetPerson(int personId)
        {
            //var response = await GetResponseFromRabbitTask<GetPersonRequest, 
            //    GetPersonResponse>(new GetPersonRequest(personId), _personsApiUrl);
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
        public async Task<IActionResult> AddPerson([FromBody] PersonDto personDto)
        {
            //var response = await GetResponseFromRabbitTask<AddPersonRequest,
            //    GetPersonResponse>(new AddPersonRequest(personDto), _personsApiUrl);
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
        public async Task<IActionResult> UpdatePerson(int personId, [FromBody] PersonDto personDto)
        {
            //var response = await GetResponseFromRabbitTask<UpdatePersonRequest, 
            //    GetPersonResponse>(new UpdatePersonRequest(personId, personDto), _personsApiUrl);
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
        public async Task<IActionResult> DeletePerson(int personId)
        {
            var isZookeeperExists = await
                GetResponseFromRabbitTask<CheckZokeepersWithSpecialityAreExistRequest,
                CheckZokeepersWithSpecialityAreExistResponse>
                (new CheckZokeepersWithSpecialityAreExistRequest(CheckType.Person, personId), 
                _zookeepersApiUrl);

            if (isZookeeperExists.IsThereZookeeperWithThisSpeciality)
                return BadRequest($"There is zookeeper with id={personId}. " +
                    "Before deleting a zookeeper, you must remove the zookeeper " +
                    "associations with all specialties.");

            //var response = await GetResponseFromRabbitTask<DeletePersonRequest, GetPersonResponse>(
            //    new DeletePersonRequest(personId), _personsApiUrl);
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
        public async Task<IActionResult> GetSubordinatePersonnel(int personId)
        {
            //var response = await GetResponseFromRabbitTask<GetSubordinatePersonnelRequest,
            //    GetPersonsResponse>(new GetSubordinatePersonnelRequest(personId), _personsApiUrl);
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
        public async Task<IActionResult> ChangeManagerForSubordinatePersonnel(
            int currentId,
            int newId
            )
        {
            //var response = await GetResponseFromRabbitTask<ChangeManagerForSubordinatePersonnelRequest,
            //    GetPersonsResponse>(new ChangeManagerForSubordinatePersonnelRequest(currentId, 
            //                                                                        newId), 
            //                                                                        _personsApiUrl);
            var response = await _receivingService.ChangeManagerForSubordinatePersonnel(currentId,
                newId);
            
            return response.Persons != null
            ? Ok(response.Persons)
            : BadRequest(response.ErrorMessage);
        }

        private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request, Uri rabbitMqUrl)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
