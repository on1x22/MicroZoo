using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.PersonsApi.Services
{
    public interface IPersonsApiService
    {
        Task<GetPersonResponse> GetPersonAsync(int personId);
        Task<GetPersonResponse> AddPersonAsync(PersonDto personDto);
        Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto);
        Task<GetPersonResponse> DeletePersonAsync(int personId);
        

        Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId);
        Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                      int newManagerId);
    }
}
