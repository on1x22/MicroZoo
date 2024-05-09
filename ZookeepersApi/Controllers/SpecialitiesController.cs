using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        //private readonly IServiceProvider _provider;
        private readonly ISpecialitiesRequestReceivingService _receivingService;
        //private readonly Uri _animalsApiUrl;
        //private readonly Uri _personsApiUrl;
        //private readonly Uri _zookeepersApiUrl;

        public SpecialitiesController(/*IServiceProvider provider, IConfiguration configuration,*/
            ISpecialitiesRequestReceivingService receivingService)
        {
            //_provider = provider;
            _receivingService = receivingService;
            //_animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            //_personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            //_zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
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
            var response = await _receivingService.AddSpecialityAsync(specialityDto);

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
            var response = await _receivingService.ChangeRelationBetweenZookeeperAndSpecialityAsync(relationId, 
                specialityDto);

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
