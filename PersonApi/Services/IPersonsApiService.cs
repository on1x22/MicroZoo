using MicroZoo.Infrastructure.MassTransit.Responses;

namespace MicroZoo.PersonsApi.Services
{
    public interface IPersonsApiService
    {
        Task<GetPersonResponse> GetPersonAsync(int personId);
    }
}
