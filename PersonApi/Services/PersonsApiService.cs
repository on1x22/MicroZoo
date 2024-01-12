using MicroZoo.Infrastructure.MassTransit.Responses;
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
    }
}
