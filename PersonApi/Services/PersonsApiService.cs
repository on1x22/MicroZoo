using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons.Dto;
using MicroZoo.PersonsApi.Repository;

namespace MicroZoo.PersonsApi.Services
{
    public class PersonsApiService : IPersonsApiService
    {
        private readonly IPersonRepository _repository;
        public PersonsApiService(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<GetPersonResponse> GetPersonAsync(int personId)
        {
            var response = new GetPersonResponse
            {
                Person = await _repository.GetPersonAsync(personId)
            };

            if (response.Person == null)
                response.ErrorMessage = $"Animal with id = {personId} not found";

            return response;
        }

        public async Task<GetPersonResponse> AddPersonAsync(PersonDto personDto)
        {
            var response = new GetPersonResponse();

            if (!await _repository.CheckPersonIsManager(personDto.ManagerId))
                response.ErrorMessage = $"Manager with id={personDto.ManagerId} is not exist";
            else
                response.Person = await _repository.AddPersonAsync(personDto);

            return response;
        }

        public async Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto)
        {
            var response = new GetPersonResponse();

            if (!await _repository.CheckPersonIsManager(personDto.ManagerId))
            {
                response.ErrorMessage = $"Manager with id={personDto.ManagerId} is not exist";
                return response;
            }
            
            response.Person = await _repository.UpdatePersonAsync(personId, personDto);

            if (response.Person == null)
                response.ErrorMessage = $"Person with id = {personId} not found";

            return response;
        }

        public async Task<GetPersonResponse> DeletePersonAsync(int personId)
        {
            var response = new GetPersonResponse
            {
                Person = await _repository.DeletePersonAsync(personId)
            };

            if (response.Person == null)
                response.ErrorMessage = $"Person with id = {personId} not found";

            return response;
        }

        public async Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId)
        {
            var response = new GetPersonsResponse
            {
                Persons = await _repository.GetSubordinatePersonnelAsync(personId)
            };

            if(response.Persons == null || response.Persons.Count() == 0)
            {
                response.Persons = null;
                response.ErrorMessage = $"Employee with id={personId} has no subordinate personnel";
            }

            return response;
        }

        
    }
}
