using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Persons.Dto;
using MicroZoo.Infrastructure.Models.Persons;

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

        [HttpGet("{personId}")]
        public async Task<IActionResult> GetPerson(int personId)
        {
            var response = await GetResponseFromRabbitTask<GetPersonRequest, 
                GetPersonResponse>(new GetPersonRequest(personId));

            return response.Person != null
                ? Ok(response.Person)
                : NotFound(response.ErrorMessage);
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] PersonDto personDto)
        {
            var response = await GetResponseFromRabbitTask<AddPersonRequest,
                GetPersonResponse>(new AddPersonRequest(personDto));

            return response.Person != null
                ? Ok(response.Person)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{personId}")]
        public async Task<IActionResult> UpdatePerson(int personId, [FromBody] PersonDto personDto)
        {
            var response = await GetResponseFromRabbitTask<UpdatePersonRequest, 
                GetPersonResponse>(new UpdatePersonRequest(personId, personDto));

            return response.Person != null
                ? Ok(response.Person)
                : BadRequest(response.ErrorMessage);
        }

        [HttpDelete("{personId}")]
        public async Task<IActionResult> DeletePerson(int personId)
        {
            var response = await GetResponseFromRabbitTask<DeletePersonRequest, GetPersonResponse>(new DeletePersonRequest(personId));
            return response.Person != null
                ? Ok(response.Person)
                : NotFound(response.ErrorMessage);
        }

        [HttpGet("{personId}/subordinatePersonnel")]
        public async Task<IActionResult> GetSubordinatePersonnel(int personId)
        {
            var response = await GetResponseFromRabbitTask<GetSubordinatePersonnelRequest,
                GetPersonsResponse>(new GetSubordinatePersonnelRequest(personId));
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
